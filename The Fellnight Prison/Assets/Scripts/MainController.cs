using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {
    public GameObject[] Spawns;

	// Use this for initialization
	void Start () {
        if (PhotonNetwork.isMasterClient) { 
            LaunchGenerator();
        }
    }

    public void LaunchGenerator()
    {
        Debug.Log("LaunchGenerator()");
        //this.gameObject.GetComponent<NewRandRoom>().enabled = true;
        if(PhotonNetwork.isMasterClient)
            this.gameObject.GetComponent<NewRandRoom>().enabled = true;
    }

    public void FinishGenerator()
    {
        this.gameObject.GetComponent<PhotonView>().RPC("SpawnPlayer", PhotonTargets.All);
        //this.gameObject.GetComponent<GlobalFunctions>().SpawnPlayer();
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void quitButton()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    [PunRPC]
    void SpawnPlayer()
    {
        GameObject _spawn = Spawns[Random.Range(0, Spawns.Length)];
        GameObject _player = PhotonNetwork.Instantiate("TempPlayer", _spawn.transform.position , Quaternion.identity, 0);
        if(_player.GetPhotonView().isMine) { _player.SetActive(true); }
    }
}
