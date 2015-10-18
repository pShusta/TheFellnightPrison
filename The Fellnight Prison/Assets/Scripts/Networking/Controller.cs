using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Pair
{
    public String[] StringArray;
    public PhotonPlayer Player;

    public Pair(string[] _stringArray, PhotonPlayer _player)
    {
        StringArray = _stringArray;
        Player = _player;
    }
}

public class Controller : MonoBehaviour {
    public GameObject LoginPanel, CreateAccountPanel, MasterConnectPanel, ConnectingPanel, CreateAccountButton, CreatePanel, GoodJobPanel;
    public GameObject[] _loginInputs, SuccessOrFail, CreateInputs;
    public PhotonView myPhotonView;

    private GameObject _view;
    private bool UsernameAvailable;


	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<LoginConnection>().PhotonConnect();
        UsernameAvailable = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReturnToLogin()
    {
        GoodJobPanel.SetActive(false);
        CreatePanel.SetActive(true);
        CreateAccountButton.SetActive(false);
        CreateAccountPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void CreateScreen()
    {
        LoginPanel.SetActive(false);
        CreateAccountPanel.SetActive(true);
    }

    public void CreateAccount()
    {
        if (CreateInputs[1].GetComponent<Text>().text == CreateInputs[2].GetComponent<Text>().text)
        {
            if (UsernameAvailable)
            {
                myPhotonView.RPC("CreateAccount", PhotonTargets.MasterClient, CreateInputs[0].GetComponent<Text>().text, CreateInputs[1].GetComponent<Text>().text);
                CreatePanel.SetActive(false);
                GoodJobPanel.SetActive(true);
            }
        }
        else
        {
            CreateInputs[3].GetComponent<Text>().text = "Passwords do not match";
            //passwords do not match
        }
    }

    public void CheckUsernameReturn(bool _go)
    {
        if (_go)
        {
            CreateAccountButton.SetActive(true);
            UsernameAvailable = true;
            CreateInputs[3].GetComponent<Text>().text = "Username Available";
            //Good Name
        }
        else
        {
            CreateInputs[3].GetComponent<Text>().text = "Username Not Available";
            //Bad Name
        }
    }

    public void CheckUsername()
    {
        myPhotonView.RPC("DbUsernameCheck", PhotonTargets.MasterClient, CreateInputs[0].GetComponent<Text>().text, myPhotonView.owner);
    }

    public void ConnectionSuccesful()
    {
        _view = PhotonNetwork.Instantiate("playerTest", new Vector3(0, 0, 0), Quaternion.identity, 0);
        myPhotonView = _view.GetComponent<PhotonView>();
        ConnectingPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void Login()
    {
        if (_loginInputs[0].GetComponent<Text>().text == "Master" && _loginInputs[1].GetComponent<Text>().text == "")
        {
            LoginPanel.SetActive(false);
            MasterConnectPanel.SetActive(true);
        }
        else
        {
            string[] loginInfo = new string[2] { _loginInputs[0].GetComponent<Text>().text, _loginInputs[1].GetComponent<Text>().text };
            _view.GetComponent<PhotonRPC>().Login(loginInfo);
        }
    }

    public void LoginResult(bool _result)
    {
        Debug.Log("Recieved reply RPC");
        if (_result)
        {
            //Login Succesful
            SuccessOrFail[0].SetActive(true);
        }
        else
        {
            //Login Fail
            SuccessOrFail[1].SetActive(true);
        }
    }
}
