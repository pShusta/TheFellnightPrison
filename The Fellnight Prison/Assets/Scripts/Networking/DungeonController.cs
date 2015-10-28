using UnityEngine;
using System.Collections;

public class DungeonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void returnToSunspear()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<NetworkV2>().returnToSunspear();
    }
}
