using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkV2 : MonoBehaviour
{
    public bool connectAsMaster, solo;
    public GameObject Solo, Master;
    private bool roomLock = false;

    public void Start()
    {

    }

    public void Update()
    {

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

        RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 6 };
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

        if (connectAsMaster)
        {
            PhotonNetwork.CreateRoom("FellnightPrisonRoom" + numRooms, roomOptions, TypedLobby.Default);
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
                        PhotonNetwork.JoinRoom(_room.name);
                        break;
                    }
                }
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

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom: " + PhotonNetwork.room.ToString());

        this.gameObject.GetComponent<Controller>().ConnectionSuccesful();
        //if (solo && !PhotonNetwork.isMasterClient)
        //{
        //    this.gameObject.GetComponent<Controller>().myPhotonView.RPC("SoloRoom", PhotonTargets.MasterClient);
        //}
    }
}
