using UnityEngine;
using System.Collections;

public class PhotonRPC : MonoBehaviour {

    private PhotonView myPhotonView;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Login(string[] loginInfo)
    {
        //myPhotonView = this.gameObject.GetComponent<PhotonView>();
        
        //Pair creds = new Pair(loginInfo, myPhotonView.owner);
        Debug.Log("Sending RPC");
        myPhotonView = this.gameObject.GetComponent<PhotonView>();
        Debug.Log("PhotonPlayer: " + myPhotonView.owner);
        myPhotonView.RPC("DbLogin", PhotonTargets.MasterClient, loginInfo, myPhotonView.owner);
    }

    [PunRPC]
    void CreateAccount(string _username, string _password)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<Database>().CreateAccount(_username, _password);
    }

    [PunRPC]
    void DbUsercheckReturn(bool _go)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<Controller>().CheckUsernameReturn(_go);
    }

    [PunRPC]
    void DbUsernameCheck(string _username, PhotonPlayer _player)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        bool _go = controller.GetComponent<Database>().CheckUsername(_username);
        Debug.Log("_go: " + _go);
        Debug.Log("_player: " + _player);
        myPhotonView = this.gameObject.GetComponent<PhotonView>();
        myPhotonView.RPC("DbUsercheckReturn", _player, _go);
    }

    [PunRPC]
    void DbLogin(string[] _creds, PhotonPlayer _player)
    {
        Debug.Log("Recieved RPC");
        string _username = _creds[0];
        string _password = _creds[1];
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        bool _go = controller.GetComponent<Database>().Login(_username, _password);
        Debug.Log("Sending reply RPC");
        Debug.Log("PhotonPlayer: " + _player);
        Debug.Log("_go: " + _go);
        myPhotonView = this.gameObject.GetComponent<PhotonView>();
        myPhotonView.RPC("LoginResult", _player, _go);
    }

    [PunRPC]
    void LoginResult(bool _result)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<Controller>().LoginResult(_result);
    }

    [PunRPC]
    void GetCharacter(string _username, PhotonPlayer _player)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        int[] _stats = controller.GetComponent<Database>().GeneratePlayerCore(_username, _player);
        myPhotonView = this.gameObject.GetComponent<PhotonView>();
        if (_stats != null)
        {
            myPhotonView.RPC("ReturnCore", _player, _stats);
        }
        else
        {
            myPhotonView.RPC("NoCharacter", _player);
        }
    }

    [PunRPC]
    void NoCharacter()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<Controller>().NoCharacter();
    }

    [PunRPC]
    void ReturnCore(int[] _stats)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<Controller>().CoreReturn(_stats);
    }
}
