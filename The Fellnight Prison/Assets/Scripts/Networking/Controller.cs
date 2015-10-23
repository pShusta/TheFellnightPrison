using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
    public GameObject LoginPanel, CreateAccountPanel, MasterConnectPanel, ConnectingPanel, CreateAccountButton, CreatePanel, GoodJobPanel, CharacterCreatePanel, PlayerMenu;
    public GameObject[] _loginInputs, SuccessOrFail, CreateInputs;
    public PhotonView myPhotonView;

    private GameObject _view;
    private bool UsernameAvailable;
    public Player _player;


	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        this.gameObject.GetComponent<NetworkV2>().PhotonConnect();
        UsernameAvailable = false;
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
            CreateInputs[0].GetComponentInParent<InputField>().interactable = false;
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
        //ConnectingPanel.SetActive(false);
        //LoginPanel.SetActive(true);
        if (_loginInputs[0].GetComponent<Text>().text == "Master" && _loginInputs[1].GetComponent<InputField>().text == "")
        {
            LoginPanel.SetActive(false);
            MasterConnectPanel.SetActive(true);
        }
        else
        {
            string[] loginInfo = new string[2] { _loginInputs[0].GetComponent<Text>().text, _loginInputs[1].GetComponent<InputField>().text };
            _view.GetComponent<PhotonRPC>().Login(loginInfo);
        }
    }

    public void Login()
    {
        if (_loginInputs[0].GetComponent<Text>().text == "Master" && _loginInputs[1].GetComponent<InputField>().text == "")
        {
            this.gameObject.GetComponent<NetworkV2>().setAsMaster();
        }
        this.gameObject.GetComponent<NetworkV2>().PhotonConnect();
        UsernameAvailable = false;
        //if (_loginInputs[0].GetComponent<Text>().text == "Master" && _loginInputs[1].GetComponent<InputField>().text == "")
        //{
        //    LoginPanel.SetActive(false);
        //    MasterConnectPanel.SetActive(true);
        //}
        //else
        //{
        //   string[] loginInfo = new string[2] { _loginInputs[0].GetComponent<Text>().text, _loginInputs[1].GetComponent<InputField>().text };
        //    _view.GetComponent<PhotonRPC>().Login(loginInfo);
        //}
    }

    public void LoginResult(bool _result)
    {
        Debug.Log("Recieved reply RPC");
        if (_result)
        {
            //Login Succesful
            SuccessOrFail[0].SetActive(true);
            
            myPhotonView.RPC("GetCharacter", PhotonTargets.MasterClient, _loginInputs[0].GetComponent<Text>().text, myPhotonView.owner);
        }
        else
        {
            //Login Fail
            SuccessOrFail[1].SetActive(true);
        }
    }

    public void NoCharacter()
    {
        LoginPanel.SetActive(false);
        CharacterCreatePanel.SetActive(true);
    }

    public void CoreReturn(int[] _stats){
        //Recieved Core stats for player
        _player = new Player(_loginInputs[0].GetComponent<Text>().text, _stats[0], _stats[1], _stats[2], _stats[3], _stats[4], _stats[5], _stats[6]);
        SuccessOrFail[0].GetComponent<Text>().text = "Player Loaded";
        myPhotonView.RPC("GetInventory", PhotonTargets.MasterClient, _loginInputs[0].GetComponent<Text>().text, myPhotonView.owner);
    }

    public void CloseMasterLogin()
    {
        MasterConnectPanel.SetActive(false);
        PlayerMenu.SetActive(true);
    }

    public void InvFilled()
    {
        SuccessOrFail[0].GetComponent<Text>().text = "Inventory Loaded";
        //Application.LoadLevel("Tavern");
        PlayerMenu.SetActive(true);
        PhotonNetwork.Instantiate("TempPlayer", GameObject.FindGameObjectWithTag("Spawnpoint").transform.position, Quaternion.identity, 0);
        LoginPanel.SetActive(false);
    }

    public void quitButton()
    {
        Application.Quit();
    }

    public void loadDungeonButton()
    {
        //PhotonNetwork.automaticallySyncScene = true;
        myPhotonView.RPC("LoadDungeonLevel", PhotonTargets.MasterClient);
    }

    void OnPhotonCustomRoomPropertiesChanged(Hashtable _changed)
    {
        Debug.Log("OnPhotonCustomRoomPropertiesChanged");
        foreach (var _value in _changed.Values)
        {
            Debug.Log("Hastable: " + _value);
        }
    }

    
}
