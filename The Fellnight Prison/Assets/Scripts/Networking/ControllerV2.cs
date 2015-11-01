using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControllerV2 : MonoBehaviour {

    public CarryData carryData;
    public GameObject carryDataPrefab, loginPanelUsername, loginPanelPassword, healthPanel, playerToon, uiController;
    public GameObject loginPanel, masterConnectPanel, loginButton, createAccountPanel, createAccountButton;
    public GameObject createPanel, goodJobPanel, _carryDataObj;
    public GameObject[] successOrFail, createInputs;
    public PhotonView view;
    public PhotonRPC rpc;

    private bool initLoad, _create, UsernameAvailable;

	void Start () {
        _carryDataObj = GameObject.FindWithTag("CarryData");
        _create = false;
        if (_carryDataObj != null)
        {
            carryData = GameObject.FindWithTag("CarryData").GetComponent<CarryData>();
        }
        else
        {
            Instantiate(carryDataPrefab, Vector3.zero, Quaternion.identity);
            carryData = GameObject.FindWithTag("CarryData").GetComponent<CarryData>();
            initLoad = true;
        }
	}

    void AutoLogin() 
    {
        carryData = _carryDataObj.GetComponent<CarryData>();

        if (carryData.username == "Master" && carryData.password == "") //Is MasterClient
        {
            this.gameObject.GetComponent<NetworkV2>().setAsMaster();
            this.gameObject.GetComponent<Database>().MasterConnect();
            if (carryData.destination != "FellnightPrisonLobby")
                this.gameObject.GetComponent<NetworkV2>().wait(10);
            
        }
        else //Is not MasterClient
        {
            Debug.Log("ControllerV2.AutoLogin().else");
            healthPanel.SetActive(true);
            playerToon = PhotonNetwork.Instantiate("SkeletonPlayer", GameObject.FindGameObjectWithTag("Spawnpoint").transform.position, Quaternion.identity, 0);
            playerToon.GetComponent<PhotonView>().owner.name = carryData.username;
            uiController.GetComponent<MenuController>().setClear(true);
        } 
    }

    public void Login()
    {
        loginButton.GetComponent<Button>().interactable = false;
        if (!PhotonNetwork.connected)
        {
            if (loginPanelUsername.GetComponent<InputField>().text == "Master" && loginPanelPassword.GetComponent<InputField>().text == "")
                this.gameObject.GetComponent<NetworkV2>().setAsMaster();
            this.gameObject.GetComponent<NetworkV2>().PhotonConnect();
            UsernameAvailable = false;
        }
    }

    public void ConnectionSuccesful()
    {
        view = PhotonNetwork.Instantiate("playerTest", Vector3.zero, Quaternion.identity, 0).GetComponent<PhotonView>();
        rpc = view.GetComponent<PhotonRPC>();
        if(_create)
        {

        }
        else if (carryData.ready)
        {
            view = PhotonNetwork.Instantiate("playerTest", Vector3.zero, Quaternion.identity, 0).GetComponent<PhotonView>();
            rpc = view.GetComponent<PhotonRPC>();
            Debug.Log("ControllerV2.ConnectionSuccesful().carryData.ready");
            if (carryData.username == "Master" && carryData.password == "")
            {
                loginPanel.SetActive(false);
                carryData.username = "Master";
                carryData.password = "";
                this.gameObject.GetComponent<Database>().MasterConnect();
            }
            else
            {
                Debug.Log("ControllerV2.ConnectionSuccesful().carryData.ready.else");
                healthPanel.SetActive(true);
                playerToon = PhotonNetwork.Instantiate("SkeletonPlayer", GameObject.FindGameObjectWithTag("Spawnpoint").transform.position, Quaternion.identity, 0);
                playerToon.GetComponent<PhotonView>().owner.name = carryData.username;
                uiController.GetComponent<MenuController>().setClear(true);
                loginPanel.SetActive(false);
            }
        }
        else if (loginPanelUsername.GetComponent<InputField>().text == "Master" && loginPanelPassword.GetComponent<InputField>().text == "")
        {
            loginPanel.SetActive(false);
            carryData.username = "Master";
            carryData.password = "";
            this.gameObject.GetComponent<Database>().MasterConnect();
        }
        else
        {
            string[] loginInfo = new string[2] { loginPanelUsername.GetComponent<InputField>().text, loginPanelPassword.GetComponent<InputField>().text };
            rpc.Login(loginInfo);
        }
    }

    public void CloseMasterLogin()
    {
        Debug.Log("ControllerV2.CloseMasterLogin()");
        if (!initLoad)
        {
            Debug.Log("ControllerV2.CloseMasterLogin().!initLoad");
            masterConnectPanel.SetActive(false);
            loginPanel.SetActive(false);
            GameObject.FindWithTag("CarryData").GetComponent<CarryData>().ready = true;
            if (carryData.destination != "FellnightPrisonLobby")
                this.gameObject.GetComponent<NetworkV2>().wait(5);
        }
    }

    public void LoginResult(bool _result)
    {
        if (_result)
        {
            successOrFail[0].SetActive(true);
            carryData.username = loginPanelUsername.GetComponent<InputField>().text;
            carryData.password = loginPanelPassword.GetComponent<InputField>().text;
            view.RPC("GetCharacter", PhotonTargets.MasterClient, carryData.username,  PhotonNetwork.player);
        }
        else
        {
            successOrFail[1].SetActive(true);
            loginButton.GetComponent<Button>().interactable = true;
        }
    }

    public void NoCharacter()
    {
        loginPanel.SetActive(false);
        //characterCreatePanel.SetActive(true);
    }

    public void CoreReturn(int[] _stats)
    {
        carryData.player = new Player(carryData.username, _stats[0], _stats[1], _stats[2], _stats[3], _stats[4], _stats[5], _stats[6]);
        successOrFail[0].GetComponent<Text>().text = "Player Loaded";
        view.owner.name = carryData.username;
        view.RPC("GetInventory", PhotonTargets.MasterClient, carryData.username, PhotonNetwork.player);
    }

    public void InvFilled()
    {
        successOrFail[0].GetComponent<Text>().text = "Inventory Loaded";
        healthPanel.SetActive(true);

        playerToon = PhotonNetwork.Instantiate("SkeletonPlayer", GameObject.FindGameObjectWithTag("Spawnpoint").transform.position, Quaternion.identity, 0);
        playerToon.GetComponent<PhotonView>().owner.name = carryData.username;
        carryData.ready = true;
        uiController.GetComponent<MenuController>().setClear(true);
        loginPanel.SetActive(false);
    }

    public void setInit(bool _value)
    {
        initLoad = _value;
    }

    public void CreateScreen()
    {
        this.gameObject.GetComponent<NetworkV2>().PhotonConnect();
        _create = true;
        loginPanel.SetActive(false);
        createAccountPanel.SetActive(true);
    }

    public void CheckUsername()
    {
        if (createInputs[0].GetComponent<Text>().text.Contains(" "))
        {
            createInputs[3].GetComponent<Text>().text = "Please remove spaces";
        }
        else
        {
            view.RPC("DbUsernameCheck", PhotonTargets.MasterClient, createInputs[0].GetComponent<Text>().text, PhotonNetwork.player);
        }
    }

    public void CheckUsernameReturn(bool _go)
    {
        if (_go)
        {
            createAccountButton.SetActive(true);
            UsernameAvailable = true;
            createInputs[3].GetComponent<Text>().text = "Username Available";
            createInputs[0].GetComponentInParent<InputField>().interactable = false;
        }
        else
        {
            createInputs[3].GetComponent<Text>().text = "Username Not Available";
        }
    }

    public void CreateAccount()
    {

        if (createInputs[1].GetComponent<InputField>().text == createInputs[2].GetComponent<InputField>().text)
        {
            if (UsernameAvailable)
            {
                view.RPC("CreateAccount", PhotonTargets.MasterClient, createInputs[0].GetComponent<Text>().text, createInputs[1].GetComponent<InputField>().text);
                createPanel.SetActive(false);
                goodJobPanel.SetActive(true);
                _create = false;
            }
        }
        else
        {
            createInputs[3].GetComponent<Text>().text = "Passwords do not match";
            //passwords do not match
        }
    }

    public void ReturnToLogin()
    {
        goodJobPanel.SetActive(false);
        createPanel.SetActive(true);
        createAccountButton.SetActive(false);
        createAccountPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
}
