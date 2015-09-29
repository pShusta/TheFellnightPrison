using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {

    public GameObject _roomGen;

	// Use this for initialization
	void Start () {
        GlobalFunctions.PhotonConnect();
    }

    public void LaunchGenerator()
    {
        _roomGen.GetComponent<NewRandRoom>().enabled = true;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
