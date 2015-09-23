using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkController : MonoBehaviour {

public GameObject CreateMenu;
public GameObject MainMenu;
public GameObject[] RoomPanels;
public GameObject[] Pages;

private List<RoomInfo> Rooms = new List<RoomInfo>();
private int NumberOfRooms;


	// Use this for initialization
	void Start () {
		if(!PhotonNetwork.connected){ PhotonNetwork.ConnectUsingSettings("V0.0"); }
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void CreateGame(){
		CreateMenu.SetActive(true);
		MainMenu.SetActive(false);
		PhotonNetwork.CreateRoom("FillerRoom-1");
	}
	
	void OnConnectedToPhoton()
	{
		//Debug.Log("Connected to Photon Server");
	}
	
	void OnJoinedLobby(){
		//Debug.Log("OnJoinedLobby");
		MainMenu.SetActive(true);
		//PhotonNetwork.CreateRoom("FillerRoom-1");
		
	}
	
	void UpdateRooms(){
		RoomInfo[] _rooms = PhotonNetwork.GetRoomList();
		foreach(RoomInfo _room in _rooms){ Debug.Log (_room.name); Rooms.Add(_room); }
		NumberOfRooms = Rooms.Count;
		Debug.Log("NumberOfRooms: " + NumberOfRooms);
		for(int i = 0; i < NumberOfRooms; i++){
			RoomPanels[i].SetActive(true);
			RoomPanels[i].GetComponentInChildren<UILabel>().text = Rooms[i].name;
		}
	}
	
	void OnReceivedRoomListUpdate(){
		UpdateRooms();
	}
	
	void OnPhotonJoinRoomFailed(){
		//Debug.Log ("OnJoinRoomFailed");
		PhotonNetwork.CreateRoom("FillerRoom-" + 1);
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
			
	}
}
