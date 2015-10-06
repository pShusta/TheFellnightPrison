using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {


	// Use this for initialization
	void Start () {
        if (!PhotonNetwork.connected) { 
            GlobalFunctions.PhotonConnect();
        }
        else
        {
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
        this.gameObject.GetComponent<GlobalFunctions>().SpawnPlayer();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
