using UnityEngine;
using System;
using System.Collections;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class NetworkBase : MonoBehaviour {

	void Start () {
		PhotonNetwork.ConnectUsingSettings("V1.0.0");
	}
	
	void Update () {
	}
	
	void OnConnectedToPhoton()
	{
		//Debug.Log("Connected to Photon Server");
	}
	
	void OnJoinedLobby(){
		//Debug.Log("OnJoinedLobby");
		PhotonNetwork.CreateRoom("BaseRoom-" + Random.Range(1, 9999));
	}
	
	void OnPhotonJoinFailed(){
		//Debug.Log ("OnJoinRoomFailed");
	}
	
	void OnPhotonCreateFailed(){
		//Debug.Log("OnCreateRoomFailed");
	}
	
	
	void OnCreatedRoom(){
		//Debug.Log("OnCreatedRoom");
	}
	
	
	void OnJoinedRoom ()
	{
		//Debug.Log("OnJoinedRoom");
		double _myPing = (double)PhotonNetwork.GetPing();
		Room _room = PhotonNetwork.room;
		
		PhotonHashtable changeProps = new PhotonHashtable();
		changeProps.Add("LowPing", _myPing);
		changeProps.Add("LowPingPlayer", PhotonNetwork.player.ID);
		if(!_room.customProperties.ContainsKey("LowPing")) { _room.SetCustomProperties(changeProps); } else if ( Convert.ToDouble(_room.customProperties.ContainsKey("LowPing")) > _myPing) { _room.SetCustomProperties(changeProps); }
		if(PhotonNetwork.isMasterClient) { 
			PhotonPlayer[] _players = PhotonNetwork.playerList;
			foreach(PhotonPlayer _checkPlayer in _players){ if(_checkPlayer.ID.ToString() == PhotonNetwork.room.customProperties["LowPingPlayer"].ToString()) { PhotonNetwork.SetMasterClient(_checkPlayer); } } 
		}
		PhotonNetwork.Instantiate("PlayerObject", GameObject.FindGameObjectWithTag("PlayerObject").transform.position, Quaternion.identity, 0);
	}
}
