using UnityEngine;
using System;
using System.Collections;
using MySql.Data.MySqlClient;

public class Database : MonoBehaviour{

    private static string serverIP = "localhost";
    private static string port = "3306";
    private static string database = "thefellnightprison";
    private static string Uid = "Fellnight";
    private static string Pwd = "Sunspear";

    void Start()
    {
        Weapon test = new Weapon();
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