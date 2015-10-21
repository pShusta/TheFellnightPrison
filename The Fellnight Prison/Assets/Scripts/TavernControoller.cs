using UnityEngine;
using System.Collections;

public class TavernControoller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void quitButton()
    {
        Application.Quit();
    }

    public void loadDungeonButton()
    {
        //PhotonNetwork.automaticallySyncScene = true;
        this.gameObject.GetComponent<PhotonView>().RPC("LoadDungeonLevel", PhotonTargets.MasterClient);
    }

    void OnPhotonCustomRoomPropertiesChanged()
    {
        Debug.Log("OnPhotonCustomRoomPropertiesChanged");
    }

    [PunRPC]
    void LoadDungeonLevel()
    {
        PhotonNetwork.automaticallySyncScene = true;
        //PhotonNetwork.LoadLevel(2);
    }
}
