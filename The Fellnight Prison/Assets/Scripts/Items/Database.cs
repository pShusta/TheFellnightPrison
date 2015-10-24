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

    public GameObject Inventory;
    public GameObject[] _masterInputs;

    public List<Player> Players = new List<Player>();

    void Start()
    {
        
    }

    public void GetInventory(string _username, PhotonPlayer _player)
    {
        
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
                _reader = _cmd.ExecuteReader();
                while (_reader.Read())
                {
                    if (t == "Weapons")
                    {
                        this.gameObject.GetComponent<Controller>().myPhotonView.RPC("RecieveWeapon", _player,
                                            _reader["idWeapons"].ToString(),
                                            _reader["WeaponName"].ToString(),
                                            _reader["DmgType"].ToString(),
                                            _reader["DmgAmt"].ToString(),
                                            _reader["EleDmgType"].ToString(),
                                            _reader["EleDmgAmt"].ToString(),
                                            _reader["WeaponRange"].ToString(),
                                            _reader["Durability"].ToString(),
                                            _reader["Weight"].ToString()
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
                        this.gameObject.GetComponent<Controller>().myPhotonView.RPC("RecieveMaterial", _player,
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
                    //Debug.Log(_reader["id" + t].ToString());
                }
                _reader.Close();
            }
            _c = "";
        }
        this.gameObject.GetComponent<Controller>().myPhotonView.RPC("InvFilled", _player);
        foreach (Player _p in Players)
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
        Players.Add(_p);
        int[] _stats = new int[] { _stats1[0], _stats1[1], _stats1[2], _stats1[3], _stats1[4], _stats2[0], _stats2[1] };
        return _stats;
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
    }

    public bool Login(string _username, string _password)
    {
        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM users.usernamepassword Where username = '" + _username + "'";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        //_reader.Read();
        Debug.Log("_reader['Passcode'] == " + _reader["Passcode"]);
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
        serverIP = _masterInputs[0].GetComponent<Text>().text;
        port = _masterInputs[1].GetComponent<Text>().text;
        Uid = _masterInputs[2].GetComponent<Text>().text;
        Pwd = _masterInputs[3].GetComponent<InputField>().text;
        database = "users";
        string source = "Server=" + serverIP + "; Port=" + port + "; Database=" + database + "; Uid=" + Uid + "; Password=" + Pwd + ";";
        _masterConnect = new MySqlConnection(source);
        _masterConnect.Open();
        Debug.Log("Connection Succesful");
        PhotonNetwork.Instantiate("SKELETON", GameObject.FindGameObjectWithTag("Spawnpoint").transform.position, Quaternion.identity, 0);
        Inventory.SetActive(true);
        this.gameObject.GetComponent<Controller>().CloseMasterLogin();
        //Application.LoadLevel("Tavern");
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
        //string source = "Server=" + serverIP + ";Database=" + database + ";Uid=" + Uid + ";Pwd=" + Pwd + ";";
        //MySqlConnection _connect = new MySqlConnection(source);
        //_connect.Open();

        //name, range, DmgType, EleDmgType, DmgAmt, EleDmgAmt, WeapType, Durability, Weight
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
        //string source = "Server=" + serverIP + ";Database=" + database + ";Uid=" + Uid + ";Pwd=" + Pwd + ";";
        //MySqlConnection _connect = new MySqlConnection(source);
        //_connect.Open();

        //name, range, DmgType, EleDmgType, DmgAmt, EleDmgAmt, WeapType, Durability, Weight
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
}