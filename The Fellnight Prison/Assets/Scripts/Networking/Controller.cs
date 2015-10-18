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
    public GameObject LoginPanel, CreateAccountPanel, MasterConnectPanel, ConnectingPanel;
    public GameObject[] _loginInputs, SuccessOrFail;
    public PhotonView myPhotonView;

    private GameObject _view;


	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<LoginConnection>().PhotonConnect();
        

	}
	
	// Update is called once per frame
	void Update () {
	
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
