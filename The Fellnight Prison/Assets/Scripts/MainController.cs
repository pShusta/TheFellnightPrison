using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {


	// Use this for initialization
	void Start () {
        GlobalFunctions.PhotonConnect();
    }

    public void LaunchGenerator()
    {
        Debug.Log("LaunchGenerator()");
        //this.gameObject.GetComponent<NewRandRoom>().enabled = true;
        this.gameObject.GetComponent<NewRandRoom>().enabled = true;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
