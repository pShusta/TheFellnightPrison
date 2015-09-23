using UnityEngine;
using System.Collections;
using MySql.Data.MySqlClient;

public class Database : MonoBehaviour {

    string source;
    MySqlConnection _connection;

	// Use this for initialization
	void Start () {
        source = "Server=localhost;Database=thefellnightprison;Uid=root;Pwd=MyloA1daine;";
        _connection = new MySqlConnection(source);
        _connection.Open();
        RunCommand(_connection);
        _connection.Close();
	}

    void RunCommand(MySqlConnection _connect)
    {
        MySqlCommand _command = _connect.CreateCommand();
        _command.CommandText = "SELECT * from weapontest";
        MySqlDataReader _reader = _command.ExecuteReader();
        while (_reader.Read())
        {
            Debug.Log("WeaponName: " + _reader["WeaponName"]);
            Debug.Log("WeaponDamage: " + _reader["WeaponDamage"]);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}

/*
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        SqlConnection sc = new SqlConnection ("Data Source=SAF-PC\\SQLEXPRESS;Integrated Security=TRUE;Initial Catalog=safa");
        SqlCommand cmd;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                sc.Open();
                cmd = new SqlCommand("Insert into emp (ID,Name) values('" + txtid.Text + "','" + txtname.Text + "')", sc);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Update Successfull to the Database");
                sc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}
*/