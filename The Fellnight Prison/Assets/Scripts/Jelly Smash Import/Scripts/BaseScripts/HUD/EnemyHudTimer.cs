using UnityEngine;
using System.Collections;

public class EnemyHudTimer : EnemyHealth {
	private GameObject thehudmain;
	
	
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("EnemyHealthGui");
		thehudmain = go;
	}
	
	// Update is called once per frame
	void Update () {
		if (healthtimer < 0)
			healthtimer = 0;
		if (healthtimer == 0)
			Debug.Log("Killin it");
			Destroy (thehudmain);
	}
}
