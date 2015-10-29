using UnityEngine;
using System.Collections;

public class DungeonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPhotonPlayerDisconnected(PhotonPlayer _player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.DestroyPlayerObjects(_player);
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
