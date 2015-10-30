using UnityEngine;
using System.Collections;

public class DungeonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPhotonPlayerConnected(PhotonPlayer _player)
    {
        GameObject.FindWithTag("GameController").GetComponent<Database>().GeneratePlayerCore(_player.name, _player);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer _player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.DestroyPlayerObjects(_player);
            foreach (Player _play in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
            {
                if (_play.Username == _player.name)
                {
                    GameObject.FindWithTag("GameController").GetComponent<Database>().pleaseSavePlayer(_player);
                    break;
                }
            }
        }
        if (PhotonNetwork.playerList.Length <= 1)
        {
            returnToSunspear();
        }
    }

    public void returnToSunspear()
    {
        GameObject.FindWithTag("CarryData").GetComponent<CarryData>().destination = "FellnightPrisonLobby";
        Application.LoadLevel(0);
        PhotonNetwork.LeaveRoom();
    }
}
