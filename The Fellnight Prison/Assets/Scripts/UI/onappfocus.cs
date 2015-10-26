using UnityEngine;
using System.Collections;

public class onappfocus : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnApplicationFocus()
    {
        Debug.Log("OnApplicationFocus");
    }
}
