using UnityEngine;
using System.Collections;

public class GlobalFunctions : MonoBehaviour {

	private bool _ready = false;
    private bool _connection = false;

    public GameObject RoomGen;

	public void SpawnPlayer(){
		GameObject _player = PhotonNetwork.Instantiate("NetworkPlayer", new Vector3(0,1,0), Quaternion.identity, 0);
		if(_player.GetPhotonView().isMine) { _player.SetActive(true); }
	}

    public bool PhotonConnection()
    {
        return _connection;
    }

	public bool CheckReady(){
		return _ready;
	}
	
	public void SetReady(bool _bool){
		_ready = _bool;
	}
	
	public void PhotonConnect(){
		PhotonNetwork.ConnectUsingSettings("V0.0");
        Debug.Log("PhotonConnect() has been called.");
	}
	
	void OnConnectedToPhoton()
	{
        _connection = true;
		Debug.Log("Connected to Photon Server");
        PhotonNetwork.JoinLobby();
	}
	
	void OnJoinedLobby(){
		Debug.Log("OnJoinedLobby");
		PhotonNetwork.CreateRoom("FillerRoom-" + 1);
	}
	
	void OnPhotonJoinRoomFailed(){
		Debug.Log ("OnJoinRoomFailed");
		PhotonNetwork.CreateRoom("FillerRoom-" + 2);
	}
	
	void OnPhotonCreateRoomFailed(){
		Debug.Log("OnCreateRoomFailed");
	}
	
	
	void OnCreatedRoom(){
		Debug.Log("OnCreatedRoom");
	}
	
	
	void OnJoinedRoom ()
	{
		Debug.Log("OnJoinedRoom: " + PhotonNetwork.room.ToString());
        RoomGen.GetComponent<RandRoom>().Launch();
	}
}
