using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class Database : MonoBehaviour{

    private static string serverIP;
    private static string port;
    private static string database;
    private static string Uid;
    private static string Pwd;
    private bool isMaster;
    private static MySqlConnection _masterConnect;

    void Start()
    {
    }

    public void GetInventory(string _username, PhotonPlayer _player)
    {
        Debug.Log("Database.GetInventory();");   
        List<int> _ids = new List<int>();
        List<string> _types = new List<string>();
        string[] _ty = new string[] { "Weapons", "Materials" };
        List<Weapon> Weapons = new List<Weapon>();
        List<CraftingMaterial> Mats = new List<CraftingMaterial>();
        string equip = "";
        Weapon Equip = null;

        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM users." + _username + "Items;";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        while (_reader.Read())
        {
            _ids.Add(Convert.ToInt32(_reader["Item"].ToString()));
            _types.Add(_reader["Type"].ToString());
            if (_reader["Equiped"].ToString() == "1")
            {
                equip = _reader["Item"].ToString();
            }
        }
        _reader.Close();
        string _c = "";
        foreach (string t in _ty)
        {
            for (int i = 0; i < _ids.Count; i++)
            {
                if (_types[i] == t)
                {
                    if (_c != "")
                    {
                        _c += " OR id" + t + " = " + _ids[i];
                    }
                    else
                    {
                        _c += " id" + t + " = " + _ids[i];
                    }
                }
            }
            Debug.Log("SELECT * FROM thefellnightprison." + t + " WHERE " + _c + ";");
            _cmd.CommandText = "SELECT * FROM thefellnightprison." + t + " WHERE " + _c + ";";
            if (_c != "")
            {
                try
                {
                    _reader.Close();
                }
                catch
                {

                }
                _reader = _cmd.ExecuteReader();
                while (_reader.Read())
                {
                    if (t == "Weapons")
                    {
                        bool _tempValue = false;
                        if (_reader["idWeapons"].ToString() == equip)
                        {
                            _tempValue = true;
                        }
                        this.gameObject.GetComponent<ControllerV2>().view.RPC("RecieveWeapon", _player,
                                            _reader["idWeapons"].ToString(),
                                            _reader["WeaponName"].ToString(),
                                            _reader["DmgType"].ToString(),
                                            _reader["DmgAmt"].ToString(),
                                            _reader["EleDmgType"].ToString(),
                                            _reader["EleDmgAmt"].ToString(),
                                            _reader["WeaponRange"].ToString(),
                                            _reader["Durability"].ToString(),
                                            _reader["Weight"].ToString(),
                                            _tempValue
                                     );
                        Weapon temp = new Weapon(Convert.ToInt32(_reader["idWeapons"].ToString()),
                                                  _reader["WeaponName"].ToString(),
                                                  PublicDataTypes.ToDmgType(_reader["DmgType"].ToString()),
                                                  Convert.ToInt32(_reader["DmgAmt"].ToString()),
                                                  PublicDataTypes.ToEleDmgType(_reader["EleDmgType"].ToString()),
                                                  Convert.ToInt32(_reader["EleDmgAmt"].ToString()),
                                                  Convert.ToInt32(_reader["WeaponRange"].ToString()),
                                                  Convert.ToInt32(_reader["Durability"].ToString()),
                                                  Convert.ToInt32(_reader["Weight"].ToString()));
                        Weapons.Add(temp);
                        if (_reader["idWeapons"].ToString() == equip)
                        {
                            Equip = temp;
                        }
                    }
                    else if (t == "Materials")
                    {
                        this.gameObject.GetComponent<ControllerV2>().view.RPC("RecieveMaterial", _player,
                                            _reader["idMaterials"].ToString(),
                                            _reader["MaterialName"].ToString(),
                                            _reader["Durability"].ToString(),
                                            _reader["Weight"].ToString()
                                     );
                        Mats.Add(new CraftingMaterial(Convert.ToInt32(_reader["idMaterials"].ToString()),
                                                      _reader["MaterialName"].ToString(),
                                                      Convert.ToInt32(_reader["Durability"].ToString()),
                                                      Convert.ToInt32(_reader["Weight"].ToString())));
                    }
                }
                _reader.Close();
            }
            _c = "";
        }
        Debug.Log("Calling InvFilled");
        this.gameObject.GetComponent<ControllerV2>().view.RPC("InvFilled", _player);
        foreach (Player _p in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
        {
            if (_p.Username == _username)
            {
                foreach(CraftingMaterial _m in Mats)
                {
                    _p.InvMaterials.Add(_m);
                }
                foreach (Weapon _w in Weapons)
                {
                    _p.InvWeapons.Add(_w);
                }
                _p.Equiped = Equip;
                break;
            }
        }
    }

    public void MasterGetInventory(string _username)
    {
        Debug.Log("Database.GetInventory();");
        List<int> _ids = new List<int>();
        List<string> _types = new List<string>();
        string[] _ty = new string[] { "Weapons", "Materials" };
        List<Weapon> Weapons = new List<Weapon>();
        List<CraftingMaterial> Mats = new List<CraftingMaterial>();
        string equip = "";
        Weapon Equip = null;

        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM users." + _username + "Items;";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        while (_reader.Read())
        {
            _ids.Add(Convert.ToInt32(_reader["Item"].ToString()));
            _types.Add(_reader["Type"].ToString());
            if (_reader["Equiped"].ToString() == "1")
            {
                equip = _reader["Item"].ToString();
            }
        }
        _reader.Close();
        string _c = "";
        foreach (string t in _ty)
        {
            for (int i = 0; i < _ids.Count; i++)
            {
                if (_types[i] == t)
                {
                    if (_c != "")
                    {
                        _c += " OR id" + t + " = " + _ids[i];
                    }
                    else
                    {
                        _c += " id" + t + " = " + _ids[i];
                    }
                }
            }
            Debug.Log("SELECT * FROM thefellnightprison." + t + " WHERE " + _c + ";");
            _cmd.CommandText = "SELECT * FROM thefellnightprison." + t + " WHERE " + _c + ";";
            if (_c != "")
            {
                try
                {
                    _reader.Close();
                }
                catch
                {

                }
                _reader = _cmd.ExecuteReader();
                while (_reader.Read())
                {
                    if (t == "Weapons")
                    {
                        Weapon temp = new Weapon(Convert.ToInt32(_reader["idWeapons"].ToString()),
                                                  _reader["WeaponName"].ToString(),
                                                  PublicDataTypes.ToDmgType(_reader["DmgType"].ToString()),
                                                  Convert.ToInt32(_reader["DmgAmt"].ToString()),
                                                  PublicDataTypes.ToEleDmgType(_reader["EleDmgType"].ToString()),
                                                  Convert.ToInt32(_reader["EleDmgAmt"].ToString()),
                                                  Convert.ToInt32(_reader["WeaponRange"].ToString()),
                                                  Convert.ToInt32(_reader["Durability"].ToString()),
                                                  Convert.ToInt32(_reader["Weight"].ToString()));
                        Weapons.Add(temp);
                        if (_reader["idWeapons"].ToString() == equip)
                        {
                            Equip = temp;
                        }
                    }
                    else if (t == "Materials")
                    {
                        Mats.Add(new CraftingMaterial(Convert.ToInt32(_reader["idMaterials"].ToString()),
                                                      _reader["MaterialName"].ToString(),
                                                      Convert.ToInt32(_reader["Durability"].ToString()),
                                                      Convert.ToInt32(_reader["Weight"].ToString())));
                    }
                }
                _reader.Close();
            }
            _c = "";
        }
        Debug.Log("Calling InvFilled");
        foreach (Player _p in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
        {
            if (_p.Username == _username)
            {
                foreach (CraftingMaterial _m in Mats)
                {
                    _p.InvMaterials.Add(_m);
                }
                foreach (Weapon _w in Weapons)
                {
                    _p.InvWeapons.Add(_w);
                }
                _p.Equiped = Equip;
                Debug.Log("Setting Equiped: " + _p.Equiped.Name);
                break;
            }
        }
    }

    public int[] GeneratePlayerCore(string _username, PhotonPlayer _player)
    {
        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM users.basestats Where username = '" + _username + "'";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        int[] _stats1;
        if (_reader.HasRows)
        {
            _stats1 = new int[] {Convert.ToInt32(_reader["Str"].ToString()), 
                                  Convert.ToInt32(_reader["Agi"].ToString()), 
                                  Convert.ToInt32(_reader["Con"].ToString()), 
                                  Convert.ToInt32(_reader["Intel"].ToString()), 
                                  Convert.ToInt32(_reader["Luck"].ToString())};
        }
        else
        {
            _reader.Close();
            return null;
        }
        _reader.Close();
        _cmd.CommandText = "SELECT * FROM users.skills Where username = '" + _username + "'";
        _reader = _cmd.ExecuteReader();
        _reader.Read();
        int[] _stats2 = new int[] {};
        if (_reader.HasRows)
        {
            _stats2 = new int[] {Convert.ToInt32(_reader["OneHandSword"].ToString()), 
                                  Convert.ToInt32(_reader["Gathering"].ToString())};
        }
        _reader.Close();
        Player _p = new Player(_username, _stats1[0], _stats1[1], _stats1[2], _stats1[3], _stats1[4], _stats2[0], _stats2[1]);
        foreach (Player _play in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
        {
            if (_play.Username == _p.Username)
            {
                GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players.Remove(_play);
                break;
            }
        }
        GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players.Add(_p);
        int[] _stats = new int[] { _stats1[0], _stats1[1], _stats1[2], _stats1[3], _stats1[4], _stats2[0], _stats2[1] };
        return _stats;
    }

    public void MasterGeneratePlayerCore(string _username)
    {
        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM users.basestats Where username = '" + _username + "'";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        int[] _stats1;
        if (_reader.HasRows)
        {
            _stats1 = new int[] {Convert.ToInt32(_reader["Str"].ToString()), 
                                  Convert.ToInt32(_reader["Agi"].ToString()), 
                                  Convert.ToInt32(_reader["Con"].ToString()), 
                                  Convert.ToInt32(_reader["Intel"].ToString()), 
                                  Convert.ToInt32(_reader["Luck"].ToString())};
            _reader.Close();
            _cmd.CommandText = "SELECT * FROM users.skills Where username = '" + _username + "'";
            _reader = _cmd.ExecuteReader();
            _reader.Read();
            int[] _stats2 = new int[] { };
            if (_reader.HasRows)
            {
                _stats2 = new int[] {Convert.ToInt32(_reader["OneHandSword"].ToString()), 
                                  Convert.ToInt32(_reader["Gathering"].ToString())};
            }
            _reader.Close();
            Player _p = new Player(_username, _stats1[0], _stats1[1], _stats1[2], _stats1[3], _stats1[4], _stats2[0], _stats2[1]);
            foreach (Player _play in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
            {
                if (_play.Username == _p.Username)
                {
                    GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players.Remove(_play);
                    break;
                }
            }
            GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players.Add(_p);
        }
        else
        {
            _reader.Close();
        }
        
    }

    public bool CheckUsername(string _username)
    {
        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM users.usernamepassword Where username = '" + _username + "'";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        if (!_reader.HasRows)
        {
            _reader.Close();
            return true;
        }
        else
        {
            _reader.Close();
            return false;
        }
    }

    public void CreateAccount(string _username, string _password)
    {
        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "INSERT INTO users.usernamepassword (Username, Passcode) VALUES ( '" + _username + "', '" + _password + "');";
        _cmd.ExecuteNonQuery();

        _cmd.CommandText = "INSERT INTO users.basestats (Username) VALUES ('" + _username + "');";
        _cmd.ExecuteNonQuery();

        _cmd.CommandText = "INSERT INTO users.skills (Username) VALUES ('" + _username + "');";
        _cmd.ExecuteNonQuery();

        _cmd.CommandText = "CREATE TABLE " + _username + "Items (Item INT(11), Type VARCHAR(45), Quantity INT(11), Equiped INT(11));";
        _cmd.ExecuteNonQuery();

        _cmd.CommandText = "INSERT INTO " + _username + "Items (Item, Type, Quantity, Equiped) VALUES (1, 'Weapons', 1, 1);";
        _cmd.ExecuteNonQuery();
    }

    public bool Login(string _username, string _password)
    {
        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM users.usernamepassword Where username = '" + _username + "'";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        if (_reader["Passcode"].ToString() == _password.ToString())
        {
            _reader.Close();
            return true;
        }
        else
        {
            _reader.Close();
            return false;
        }
    }

    public void MasterConnect()
    {
        serverIP = "localhost";
        port = "3306";
        Uid = "Fellnight";
        Pwd = "Sunspear";
        database = "users";
        string source = "Server=" + serverIP + "; Port=" + port + "; Database=" + database + "; Uid=" + Uid + "; Password=" + Pwd + ";";
        _masterConnect = new MySqlConnection(source);
        _masterConnect.Open();
        Debug.Log("Connection Succesful");
        PhotonNetwork.Instantiate("GM", GameObject.FindGameObjectWithTag("Spawnpoint").transform.position, Quaternion.identity, 0);
        GameObject.FindWithTag("MenuController").GetComponent<MenuController>().setClear(true);
        this.gameObject.GetComponent<ControllerV2>().CloseMasterLogin();
    }

    public static  Weapon ReadWeaponDb(int id)
    {

        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM thefellnightprison.weapons Where idWeapons = '" + id + "'";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        Weapon _weap = new Weapon(      Convert.ToInt32(_reader["idWeapons"].ToString()),
                                        _reader["WeaponName"].ToString(),
                                        PublicDataTypes.ToDmgType(_reader["DmgType"].ToString()),
                                        Convert.ToInt32(_reader["DmgAmt"].ToString()),
                                        PublicDataTypes.ToEleDmgType(_reader["EleDmgType"].ToString()),
                                        Convert.ToInt32(_reader["EleDmgAmt"].ToString()),
                                        Convert.ToInt32(_reader["WeaponRange"].ToString()),
                                        Convert.ToInt32(_reader["Durability"].ToString()),
                                        Convert.ToInt32(_reader["Weight"].ToString())
                                 );
        _reader.Close();
        return _weap;
    }

    public static int WeaponDbInsert(Weapon _weap)
    {
        string _fill = "', '";
        string _name, _dmgt, _elet, _weapt;
        int _range, _dmga, _elea, _dura, _wght;
        _name = _weap.GetName();
        _range = _weap.GetRange();
        _dmgt = _weap.GetDmgType().ToString();
        _elet = _weap.GetEleDmgType().ToString();
        _weapt = _weap.GetWeapType().ToString();
        _dmga = _weap.GetPhysDmgAmt();
        _elea = _weap.GetEleDmgAmt();
        _dura = _weap.GetDura();
        _wght = _weap.GetWeight();

        string values = "('" + _name + _fill + _range + _fill + _elet + _fill + _dmgt + _fill + _elea + _fill + _dmga + _fill + _weapt + _fill + _dura + _fill + _wght + "')";
        string locations = "(WeaponName, WeaponRange, EleDmgType, DmgType, EleDmgAmt, DmgAmt, WeapType, Durability, Weight)";
        string cmdText = "INSERT INTO weapons " + locations + " VALUES" + values;
        
        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = cmdText;
        _cmd.ExecuteNonQuery();

        _cmd.CommandText = "SELECT * FROM thefellnightprison.weapons Where WeaponName = '" +
                                _name + "' AND WeaponRange = '" +
                                _range + "' AND DmgType = '" +
                                _dmgt + "' AND EleDmgType = '" +
                                _elet + "' AND DmgAmt = '" +
                                _dmga + "' AND EleDmgAmt = '" +
                                _elea + "' AND Durability = '" +
                                _dura + "' AND Weight = '" +
                                _wght + "'";

        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        string thing = _reader["idWeapons"].ToString();
        
        _reader.Close();

        return Convert.ToInt32(thing);
    }

    public static int MaterialDbInsert(CraftingMaterial _weap)
    {
        string _fill = "', '";
        string _name;
        int _dura, _wght;
        _name = _weap.GetName();
        _dura = _weap.GetDura();
        _wght = _weap.GetWeight();

        string values = "('" + _name + _fill + _dura + _fill + _wght + "')";
        string locations = "(MaterialName, Durability, Weight)";
        string cmdText = "INSERT INTO Materials " + locations + " VALUES" + values;

        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = cmdText;
        _cmd.ExecuteNonQuery();

        _cmd.CommandText = "SELECT * FROM thefellnightprison.Materials Where MaterialName = '" +
                                _name + "' AND Durability = '" +
                                _dura + "' AND Weight = '" +
                                _wght + "'";

        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        string thing = _reader["idWeapons"].ToString();

        _reader.Close();

        return Convert.ToInt32(thing);
    }

    public static CraftingMaterial ReadMaterialDb(int id)
    {

        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM thefellnightprison.Materials Where idMaterial = '" + id + "'";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        CraftingMaterial _mat = new CraftingMaterial(Convert.ToInt32(_reader["idMaterials"].ToString()),
                                        _reader["MaterialName"].ToString(),
                                        Convert.ToInt32(_reader["Durability"].ToString()),
                                        Convert.ToInt32(_reader["Weight"].ToString())
                                 );
        _reader.Close();
        return _mat;
    }

    public void updateWeaponEquiped(double _weap, double _weap2, string username)
    {
        updateWeaponEquip(_weap, _weap2, username);
    }

    public static void updateWeaponEquip(double _weap, double _weap2, string username)
    {
        foreach (Player _p in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
        {
            if (_p.Username == username)
            {
                foreach (Weapon _w in _p.InvWeapons)
                {
                    if (_w.Id == _weap)
                    {
                        _p.Equiped = _w;
                        break;
                    }
                }
                break;
            }
        }

        Debug.Log("Changing Equiped");
        MySqlCommand _cmd = _masterConnect.CreateCommand();

        string cmdText = "UPDATE users." + username + "items SET Equiped = 1 WHERE Type = 'Weapons' AND Item = " + _weap + ";";
        _cmd.CommandText = cmdText;
        _cmd.ExecuteNonQuery();

        cmdText = "UPDATE users." + username + "items SET Equiped = 0 WHERE Type = 'Weapons' AND Item = " + _weap2 + ";";
        _cmd.CommandText = cmdText;
        _cmd.ExecuteNonQuery();
 
    }

    public void dropWeapon(double _weap, string username)
    {
        dropWeaponNow(_weap, username);
    }

    public static void dropWeaponNow(double _weap, string username)
    {
        foreach (Player _p in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
        {
            if (_p.Username == username)
            {
                foreach (Weapon _w in _p.InvWeapons)
                {
                    if (_w.Id == _weap)
                    {
                        _p.InvWeapons.Remove(_w);
                        break;
                    }
                }
                break;
            }
        }

        Debug.Log("Dropping Weapon");
        MySqlCommand _cmd = _masterConnect.CreateCommand();

        string cmdText = "DELETE FROM users." + username + "items WHERE Type = 'Weapons' AND Item = " + _weap + ";";
        _cmd.CommandText = cmdText;
        _cmd.ExecuteNonQuery();
    }

    public void pleaseSavePlayers(PhotonPlayer[] _players)
    {
        savePlayers(_players);
    }

    public static void savePlayers(PhotonPlayer[] _players)
    {
        foreach (PhotonPlayer _p in _players)
        {
            foreach (Player _p2 in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
            {
                if (_p2.Username == _p.name)
                {
                    MySqlCommand _cmd = _masterConnect.CreateCommand();

                    string cmdText = "UPDATE users.skills SET OneHandSword = " + _p2.OneHandedSword + " AND Gathering = " + _p2.Gathering + " WHERE username = " + _p.name + ";";
                    _cmd.CommandText = cmdText;
                    _cmd.ExecuteNonQuery();

                    cmdText = "UPDATE users.basestats SET Str = " + _p2.Str + " AND Agi = " + _p2.Agi + " AND Con = " + _p2.Con + " AND Intel = " + _p2.Intel + " AND Luck = " + _p2.Luck + " WHERE username = " + _p.name + ";";
                    _cmd.CommandText = cmdText;
                    _cmd.ExecuteNonQuery();
                    break;
                }
            }
        }
    }

    public void pleaseSavePlayer(PhotonPlayer _player){
        savePlayer(_player);
    }

    public static void savePlayer(PhotonPlayer _players)
    {

            foreach (Player _p2 in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
            {
                if (_p2.Username == _players.name)
                {
                    MySqlCommand _cmd = _masterConnect.CreateCommand();

                    string cmdText = "UPDATE users.skills SET OneHandSword = " + _p2.OneHandedSword + " AND Gathering = " + _p2.Gathering + " WHERE username = " + _p2.Username + ";";
                    _cmd.CommandText = cmdText;
                    _cmd.ExecuteNonQuery();

                    cmdText = "UPDATE users.basestats SET Str = " + _p2.Str + " AND Agi = " + _p2.Agi + " AND Con = " + _p2.Con + " AND Intel = " + _p2.Intel + " AND Luck = " + _p2.Luck + " WHERE username = " + _p2.Username + ";";
                    _cmd.CommandText = cmdText;
                    _cmd.ExecuteNonQuery();
                    break;
                }
            }
    }
}