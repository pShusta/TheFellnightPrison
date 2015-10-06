using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkController : MonoBehaviour {

public GameObject CreateMenu;
public GameObject MainMenu, debugtext;
public GameObject[] RoomPanels;
public GameObject[] Pages;

private List<RoomInfo> Rooms = new List<RoomInfo>();
private int NumberOfRooms;
private double timer;


	// Use this for initialization
	void Start () {
		if(!PhotonNetwork.connected){ PhotonNetwork.ConnectUsingSettings("V0.0"); }
        timer = 3;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            UpdateRooms();
            timer = 3;
        }
	}
	
	public void CreateGame(){
		CreateMenu.SetActive(true);
		MainMenu.SetActive(false);
        Debug.Log(PhotonNetwork.countOfPlayersOnMaster);
        debugtext.GetComponent<Text>().text += PhotonNetwork.countOfPlayersOnMaster;
        
        RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 4 };
		PhotonNetwork.CreateRoom("FillerRoom-", roomOptions, null);
	}
	
	void OnConnectedToPhoton()
	{
		Debug.Log("Connected to Photon Server");
        debugtext.GetComponent<Text>().text += "Connected to Photon Server";
        PhotonNetwork.JoinLobby();
	}
	
	void OnJoinedLobby(){
		Debug.Log("OnJoinedLobby");
        debugtext.GetComponent<Text>().text += "OnJoinedLobby";
		MainMenu.SetActive(true);
		//PhotonNetwork.CreateRoom("FillerRoom-1");
		
	}
	
	void UpdateRooms(){
		RoomInfo[] _rooms = PhotonNetwork.GetRoomList();
        //Debug.Log("GetRoomList().Length: " + PhotonNetwork.GetRoomList().Length);
        //debugtext.GetComponent<Text>().text += "GetRoomList().Length: " + PhotonNetwork.GetRoomList().Length;
		foreach(RoomInfo _room in _rooms){ Debug.Log (_room.name); Rooms.Add(_room); }
		NumberOfRooms = Rooms.Count;
		//Debug.Log("NumberOfRooms: " + NumberOfRooms);
        //debugtext.GetComponent<Text>().text += "NumberOfRooms: " + NumberOfRooms;
		for(int i = 0; i < NumberOfRooms; i++){
			RoomPanels[i].SetActive(true);
			RoomPanels[i].GetComponentInChildren<UILabel>().text = Rooms[i].name;
		}
	}

    void OnRoomListUpdate()
    {
        Debug.Log("GetRoomList().Length: " + PhotonNetwork.GetRoomList().Length);
        debugtext.GetComponent<Text>().text += "GetRoomList().Length: " + PhotonNetwork.GetRoomList().Length;
    }

	void OnPhotonJoinRoomFailed(){
		Debug.Log ("OnJoinRoomFailed");
		PhotonNetwork.CreateRoom("FillerRoom-");
	}
	
	void OnPhotonCreateRoomFailed(){
		Debug.Log("OnCreateRoomFailed");
	}
	
	
	void OnCreatedRoom(){
		Debug.Log("OnCreatedRoom");
        debugtext.GetComponent<Text>().text += "OnCreatedRoom";
        Debug.Log(PhotonNetwork.room.name);
        debugtext.GetComponent<Text>().text += PhotonNetwork.room.name;
	}
	
	
	void OnJoinedRoom ()
	{
		Debug.Log("OnJoinedRoom: " + PhotonNetwork.room.ToString());
        debugtext.GetComponent<Text>().text += "OnJoinedRoom: " + PhotonNetwork.room.ToString();
			
	}

    public void Go() 
    {
        Application.LoadLevel("Dungeon");
    }

}
