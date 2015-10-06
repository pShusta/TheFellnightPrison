using UnityEngine;
using System.Collections;

public class HudTimer : PlayerHealth {
	private GameObject thehudmain;
    private float hudtimer;
	
	
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Hud");
		thehudmain = go;
	}
	
	// Update is called once per frame
	void Update () {
		if (hudtimer < 0)
			hudtimer = 0;
		if (hudtimer == 0)
			Destroy (thehudmain);
	}
}
