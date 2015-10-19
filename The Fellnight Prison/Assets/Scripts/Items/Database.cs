﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using MySql.Data.MySqlClient;

public class Database : MonoBehaviour{

    private static string serverIP;
    private static string port;
    private static string database;
    private static string Uid;
    private static string Pwd;
    private bool isMaster;
    private MySqlConnection _masterConnect;

    public GameObject[] _masterInputs;

    void Start()
    {
        
    }

    public static void setServer(string _server) { serverIP = _server; }
    public static string getServer() { return serverIP;  }
    public static void setDatabase(string _base) { database = _base;  }
    public static string getDatabase() { return database;  }
    public static void setUid(string _Uid) { Uid = _Uid;  }
    public static string getUid() { return Uid;  }
    public static void setPwd(string _Pwd) { Pwd = _Pwd;  }
    public static void setPort(string _port) { port = _port;  }
    public static string getPort() { return port; }

    public int[] GeneratePlayerCore(string _username, PhotonPlayer _player)
    {
        MySqlCommand _cmd = _masterConnect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM users.basestats Where username = '" + _username + "'";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        int[] _stats;
        if (_reader.HasRows)
        {
            _stats = new int[] {Convert.ToInt32(_reader["Str"].ToString()), 
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

        _cmd.CommandText = "INSERT INTO users.basestats (Username, Str, Agi, Con, Intel, Luck) VALUES ('" + _username + "', 10, 10, 10, 10, 0);";
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
        Pwd = _masterInputs[3].GetComponent<Text>().text;
        database = "users";
        string source = "Server=" + serverIP + "; Port=" + port + "; Database=" + database + "; Uid=" + Uid + "; Password=" + Pwd + ";";
        _masterConnect = new MySqlConnection(source);
        _masterConnect.Open();
        Debug.Log("Connection Succesful");
    }

    public static Weapon ReadWeaponDb(int id)
    {
        string source = "Server=" + serverIP + "; Port=" + port + "; Database=" + database + "; Uid=" + Uid + "; Password=" + Pwd + ";";
        MySqlConnection _connect = new MySqlConnection(source);
        _connect.Open();

        MySqlCommand _cmd = _connect.CreateCommand();
        _cmd.CommandText = "SELECT * FROM thefellnightprison.weapons Where idWeapons = '" + id + "'";
        MySqlDataReader _reader = _cmd.ExecuteReader();
        _reader.Read();
        Weapon _weap = new Weapon(      _reader["WeaponName"].ToString(),
                                        PublicDataTypes.ToDmgType(_reader["DmgType"].ToString()),
                                        Convert.ToInt32(_reader["DmgAmt"].ToString()),
                                        PublicDataTypes.ToEleDmgType(_reader["EleDmgType"].ToString()),
                                        Convert.ToInt32(_reader["EleDmgAmt"].ToString()),
                                        Convert.ToInt32(_reader["WeaponRange"].ToString()),
                                        Convert.ToInt32(_reader["Durability"].ToString()),
                                        Convert.ToInt32(_reader["Weight"].ToString())
                                 );
        _reader.Close();
        _connect.Close();

        return _weap;

    }

    public static int WeaponDbInsert(Weapon _weap)
    {
        string source = "Server=" + serverIP + ";Database=" + database + ";Uid=" + Uid + ";Pwd=" + Pwd + ";";
        MySqlConnection _connect = new MySqlConnection(source);
        _connect.Open();

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
        
        MySqlCommand _cmd = _connect.CreateCommand();
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
        _connect.Close();

        return Convert.ToInt32(thing);
    }
}