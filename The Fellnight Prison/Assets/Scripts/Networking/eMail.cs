using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class eMail : MonoBehaviour
{
    public GameObject _message, _username;

    void Start()
    {
        
    }

    public void sendMail()
    {
        
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("TheFellnightPrisonDevTeam@gmail.com");
        mail.To.Add("pshusta@gmail.com");
        mail.Subject = "The Fellnight Prison Report from " + _username;
        mail.Body = _message.GetComponent<InputField>().text;
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("TheFellnightPrisonDevTeam@gmail.com", "FellnightDev") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        smtpServer.Send(mail);
    }
}