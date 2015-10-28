using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkV2 : MonoBehaviour
{
    public bool connectAsMaster, solo, GM;
    private bool initialLoad = true;
    private bool initialLoad2 = true;
    public GameObject Solo, Master;
    private bool roomLock = false;
    private float? timer = null;
    private string roomName;

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        if (timer != null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = null;
                launchDungeon();
            }
        }
    }

    public void setSolo()
    {
        solo = Solo.GetComponent<Toggle>().isOn;
    }

    public void setAsMaster()
    {
        connectAsMaster = true;
    }

    public void PhotonConnect()
    {
        PhotonNetwork.autoJoinLobby = false;
        bool _connect = PhotonNetwork.ConnectUsingSettings("V0.0");
    }

    void OnFailedToConnectToPhoton()
    {
        Debug.Log("OnFailedToConnectToPhoton()");
    }

    void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster()");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    void OnConnectionFail()
    {
        Debug.Log("OnConnectionFail()");
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton()");
    }

    void OnConnectedToPhoton()
    {
        Debug.Log("Connected to Photon Server");
    }

    void OnReceivedRoomListUpdate()
    {
        Debug.Log("OnRecievedRoomListUpdate");

        RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 20 };
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        int numRooms = 0;
        try
        {
            numRooms = rooms.Length;
            Debug.Log("Set numRooms to " + numRooms);
        }
        catch
        {
            Debug.Log("set numRooms failed.");
        }

        if (initialLoad)
        {
            if (connectAsMaster)
            {
                initialLoad = false;
                PhotonNetwork.JoinOrCreateRoom("FellnightPrisonLobby", roomOptions, TypedLobby.Default);
            }
            else
            {
                Debug.Log("Connecting as non-Master client");
                if (solo)
                {
                    foreach (RoomInfo _room in rooms)
                    {
                        if (_room.playerCount == 1 && _room.open)
                        {
                            initialLoad = false;
                            PhotonNetwork.JoinRoom(_room.name);
                            break;
                        }
                    }
                }
                else
                {
                    foreach (RoomInfo _room in rooms)
                    {
                        if (_room.playerCount < _room.maxPlayers && _room.open)
                        {
                            initialLoad = false;
                            PhotonNetwork.JoinRoom(_room.name);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if (GM)
            {
                roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 7 };
                this.gameObject.GetComponent<Controller>().setInit(false);
                PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
            }
            else
            {
                this.gameObject.GetComponent<Controller>().setInit(false);
                PhotonNetwork.JoinRoom(roomName);
            }
        }
    }

    void OnPhotonPlayerDisconnected()
    {
        if (PhotonNetwork.room.open == false && !roomLock)
        {
            PhotonNetwork.room.open = true;
        }
    }

    void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.automaticallySyncScene = true;
    }

    void OnPhotonJoinRoomFailed(object[] error)
    {
        Debug.Log("OnPhotonJoinRoomFailed");
        Debug.Log("Error Code: " + error[0].ToString() + " Message: " + error[1]);
    }

    void OnPhotonCreateRoomFailed(object[] error)
    {
        Debug.Log("OnPhotonCreateRoomFailed");
        Debug.Log("Error Code: " + error[0].ToString() + " Message: " + error[1]);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer _player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.DestroyPlayerObjects(_player);
        }
    }

    void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    void OnPhotonPlayerDisconnect()
    {
        if(PhotonNetwork.playerList.Length <= 1){
            returnToSunspear();
        }
    }

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom: " + PhotonNetwork.room.ToString());
        //if(!initialLoad2)
        //    this.gameObject.GetComponent<Controller>().setInit(false);
        this.gameObject.GetComponent<Controller>().ConnectionSuccesful();

            //if (GM)
            //{
            //   wait(5);
            //}
        //if (solo && !PhotonNetwork.isMasterClient)
        //{
        //    this.gameObject.GetComponent<Controller>().myPhotonView.RPC("SoloRoom", PhotonTargets.MasterClient);
        //}
    }

    public void wait(float _time)
    {
        timer = _time;
    }

    void launchDungeon()
    {
        //PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LoadLevel(1);
    }

    
    public void gmHostDungeon(string _roomName)
    {
        initialLoad = false;
        GM = true;
        roomName = _roomName;
        PhotonNetwork.LeaveRoom();

        //wait(10);
    }
    
    public void loadRoom(string _roomName)
    {
        initialLoad = false;
        initialLoad2 = false;
        roomName = _roomName;
        PhotonNetwork.LeaveRoom();
        //RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 7 };
    }

    public void returnToSunspear()
    {
        initialLoad = true;
        initialLoad2 = true;
        //this.gameObject.GetComponent<Controller>().loadInit = false;
        PhotonNetwork.LeaveRoom();
    }
}
