/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Editor.Versioning
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Xml.Linq;
    using Apex.DataStructures;
    using UnityEditor;
    using UnityEngine;

    public static class ProductManager
    {
        private static Dictionary<string, Texture> _iconTextures = new Dictionary<string, Texture>();
        private static bool _assetsDownloaded;

        internal static void GetProductsInfoAsync(ApexSettings settings, bool checkForUpdates, Action<IEnumerable<ProductInfo>> callback)
        {
            EditorAsync.Execute(
                () => GetProductsInfo(settings, checkForUpdates),
                products =>
                {
                    if (settings.isDirty)
                    {
                        settings.SaveChanges();
                    }

                    if (_assetsDownloaded)
                    {
                        _assetsDownloaded = false;
                        AssetDatabase.Refresh();
                    }

                    callback(products);
                });
        }

        internal static Texture GetIcon(ProductInfo p)
        {
            return GetIcon(p.iconInfo);
        }

        internal static Texture GetIcon(IconInfo info)
        {
            Texture icon;
            if (_iconTextures.TryGetValue(info.key, out icon))
            {
                return icon;
            }

            icon = AssetDatabase.LoadMainAssetAtPath(info.path) as Texture;
            if (icon != null)
            {
                _iconTextures[info.key] = icon;
            }

            return icon;
        }

        private static IEnumerable<ProductInfo> GetProductsInfo(ApexSettings settings, bool checkForUpdates)
        {
            var productsInfo = new List<ProductInfo>();

            //Get the currently installed products
            var installed = typeof(ProductManager).Assembly.GetCustomAttributes(typeof(ApexProductAttribute), true).Cast<ApexProductAttribute>();
            foreach (var p in installed)
            {
                var info = new ProductInfo
                {
                    name = p.name,
                    installedVersion = string.IsNullOrEmpty(p.version) ? null : new Version(p.version),
                    type = p.type
                };

                info.iconInfo = GetIconPath(settings, info);
                info.changeLogPath = GetChangeLogPath(settings, info);
                productsInfo.Add(info);
            }

            //Check existence of product manifest
            var manifestPath = string.Concat(settings.dataFolder, "/Products.manifest");
            bool checkedForUpdates = checkForUpdates || settings.timeToUpdate || (!File.Exists(manifestPath) && settings.allowAutomaticUpdateCheck);
            if (checkedForUpdates)
            {
                DownloadLatest(settings, manifestPath);
            }

            //Read the product manifest
            if (!File.Exists(manifestPath))
            {
                return productsInfo;
            }

            var installedLookup = productsInfo.ToDictionary(p => p.name, StringComparer.OrdinalIgnoreCase);

            try
            {
                var productsXml = XDocument.Load(manifestPath);
                XNamespace ns = productsXml.Root.Attribute("xmlns").Value;

                foreach (var p in productsXml.Root.Elements(ns + "Product"))
                {
                    var productName = p.Element(ns + "name").Value;

                    ProductInfo info;
                    if (!installedLookup.TryGetValue(productName, out info))
                    {
                        info = new ProductInfo();
                        productsInfo.Add(info);
                    }

                    var versionString = p.Element(ns + "version").Value;
                    var patchString = p.Element(ns + "latestPatch").Value;

                    info.name = productName;
                    info.description = p.Element(ns + "description").Value;
                    info.newestVersion = string.IsNullOrEmpty(versionString) ? null : new Version(versionString);
                    info.latestPatch = string.IsNullOrEmpty(patchString) ? null : new Version(patchString);
                    info.productUrl = p.Element(ns + "productUrl").Value;
                    info.storeUrl = p.Element(ns + "storeUrl").Value;
                    info.type = (ProductType)Enum.Parse(typeof(ProductType), p.Element(ns + "type").Value);
                    info.iconInfo = GetIconPath(settings, info);
                }
            }
            catch
            {
                //Just eat this, it should not happen, but if it does we do not want to impact the users, and there is no way to recover so...
            }

            //Apply update knowledge to list
            if (checkedForUpdates)
            {
                MarkPendingUpdates(settings, productsInfo);
                EnsureIcons(settings, productsInfo);
            }

            return productsInfo;
        }

        private static IconInfo GetIconPath(ApexSettings settings, ProductInfo p)
        {
            string key = (p.type == ProductType.Product) ? p.name.Replace(" ", string.Empty) : p.type.ToString();
            return new IconInfo
            {
                key = key,
                path = string.Concat(settings.relativeDataFolder, "/", key, ".png")
            };
        }

        private static string GetChangeLogPath(ApexSettings settings, ProductInfo p)
        {
            var subFolder = p.name;
            if (!subFolder.StartsWith("Apex "))
            {
                subFolder = string.Concat("Apex ", subFolder);
            }

            var path = string.Concat(settings.rootFolder, "/", subFolder, "/ChangeLog.txt");
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        private static void MarkPendingUpdates(ApexSettings settings, IEnumerable<ProductInfo> products)
        {
            var knownPending = settings.GetKnownPendingUpdates();
            var newPending = new List<string>();

            foreach (var p in products)
            {
                if (p.status == ProductStatus.UpToDate || p.status == ProductStatus.ComingSoon)
                {
                    continue;
                }

                //For updates we alert when a new version arrives, even if the previous version was not yet updated to
                //For new products we only alert once
                string key = (p.status == ProductStatus.UpdateAvailable) ? string.Concat(p.name, "@", p.newestVersion) : p.name;
                newPending.Add(key);

                p.newUpdateAvailable = !knownPending.Contains(key);
            }

            settings.UpdateKnowPendingUpdates(newPending.ToArray());
        }

        private static void DownloadLatest(ApexSettings settings, string manifestPath)
        {
            var client = new WebClient
            {
                BaseAddress = EnsureTrailingSlash(settings.updateCheckBaseUrl)
            };

            string serviceUrl = null;

            try
            {
                client.Headers.Add("Accept", "application/xml");
                var latestUpdate = XmlSingleValue(client.DownloadString("api/Service/GetLatestUpdateTime"));

                if (settings.lastUpdateCheck >= DateTime.Parse(latestUpdate, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal))
                {
                    settings.UpdateCompleted(null);
                    return;
                }

                client.Headers.Add("Accept", "application/xml");
                var productManifest = client.DownloadString("api/Products/GetProducts");
                using (var w = new StreamWriter(manifestPath))
                {
                    w.Write(productManifest);
                }

                client.Headers.Add("Accept", "application/xml");
                serviceUrl = XmlSingleValue(client.DownloadString("api/Service/GetServiceUrl"));
            }
            catch
            {
                return;
            }

            settings.UpdateCompleted(serviceUrl);
        }

        internal static void ApplyPatches(ProductInfo p, ApexSettings settings)
        {
            var client = new WebClient
            {
                BaseAddress = EnsureTrailingSlash(settings.updateCheckBaseUrl)
            };

            client.Headers.Add("Accept", "application/xml");
            var request = string.Format("api/Products/GetAvailablePatches?productName={0}&version={1}", p.name, p.installedVersion);
            var patchList = client.DownloadString(request);

            var doc = XDocument.Parse(patchList);
            var ns = XNamespace.Get(doc.Root.Attribute("xmlns").Value);
            var patchFiles = from s in XDocument.Parse(patchList).Root.Elements(ns + "string")
                             select s.Value;

            foreach (var patchFile in patchFiles)
            {
                var patchPath = string.Concat(settings.dataFolder, "/", patchFile);
                var url = string.Concat("content/patches/", patchFile);
                client.DownloadFile(url, patchPath);

                AssetDatabase.ImportPackage(patchPath, false);
            }
        }

        internal static void EnsureIcons(ApexSettings settings, IEnumerable<ProductInfo> products)
        {
            foreach (var p in products)
            {
                if (_iconTextures.ContainsKey(p.iconInfo.key))
                {
                    continue;
                }

                string fileLocation = string.Concat(settings.dataFolder, "/", p.iconInfo.key, ".png");
                if (!File.Exists(fileLocation))
                {
                    try
                    {
                        var client = new WebClient
                        {
                            BaseAddress = EnsureTrailingSlash(settings.updateCheckBaseUrl)
                        };

                        string webLocationIcon = string.Concat("content/producticons/", p.iconInfo.key, ".png");
                        client.DownloadFile(webLocationIcon, fileLocation);

                        fileLocation = string.Concat(fileLocation, ".meta");
                        string webLocationMeta = string.Concat(webLocationIcon, ".meta");
                        client.DownloadFile(webLocationMeta, fileLocation);

                        _assetsDownloaded = true;
                    }
                    catch
                    {
                        /* not much to do about it.. */
                    }
                }
            }
        }

        private static string EnsureTrailingSlash(string url)
        {
            if (url.EndsWith("/"))
            {
                return url;
            }

            return url + "/";
        }

        private static string XmlSingleValue(string xml)
        {
            var doc = XDocument.Parse(xml);
            if (doc == null || doc.Root == null)
            {
                return string.Empty;
            }

            return doc.Root.Value;
        }
    }
}
