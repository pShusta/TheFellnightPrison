using UnityEngine;
using System.Collections;

public class BarPlayerMana : PlayerHealth {
	private GameObject thehudmana;
	
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("HudMana");
		thehudmana = go;
	}
	
	void Update () {
		if (hudtimer < 0)
			hudtimer = 0;
		if (hudtimer == 0)
			Destroy (thehudmana);
	}
}
