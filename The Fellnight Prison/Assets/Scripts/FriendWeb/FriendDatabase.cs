using UnityEngine;
using System;
using System.Collections;
using MySql.Data.MySqlClient;
using System.IO;
using System.Collections.Generic;

public class FriendDatabase : MonoBehaviour {

    private static string serverIP = "localhost";
    private static string port = "3306";
    private static string database = "friends";
    private static string Uid = "Fellnight";
    private static string Pwd = "Sunspear";

    private List<FriendClass> friends = new List<FriendClass>();

	// Use this for initialization
	void Start () {
        readSheet();
        writeDb();
	}

    void readSheet()
    {
            using (StreamReader sr = new StreamReader("data.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(',');
                    friends.Add(new FriendClass(line[2], line[3], line[4], line[5], line[6], line[1]));
                }
            }
    }

    void writeDb()
    {
        foreach(FriendClass f in friends)
        {
            writeFriend(f);
        }
    }

    void writeFriend(FriendClass f)
    {
        string source = "Server=" + serverIP + "; Port=" + port + "; Database=" + database + "; Uid=" + Uid + "; Password=" + Pwd + ";";
        MySqlConnection _connect = new MySqlConnection(source);
        _connect.Open();

        string _fill = "', '";
        string values = "('" + f.getname() + _fill + f.getf1() + _fill + f.getf2() + _fill + f.getf3() + _fill + f.getf4() + _fill + f.getf5() + "')";
        string locations = "(Name, FriendOne, FriendTwo, FriendThree, FriendFour, FriendFive)";
        string cmdText = "INSERT INTO frienddata " + locations + " VALUES" + values;

        MySqlCommand _cmd = _connect.CreateCommand();
        _cmd.CommandText = cmdText;
        _cmd.ExecuteNonQuery();

        _connect.Close();
    }

	void Update () {
	
	}
}
