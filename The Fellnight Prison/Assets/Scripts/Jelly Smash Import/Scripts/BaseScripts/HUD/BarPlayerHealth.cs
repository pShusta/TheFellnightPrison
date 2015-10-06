using UnityEngine;
using System.Collections;

public class BarPlayerHealth : PlayerHealth {
	public Texture2D[] hudgraphics;
	private float displaygraphicfloat;
	private int displaygraphic;
	private GameObject thehud;
	private float zerohpcheck;
    private float hudtimer;
    private int playercurrenthealth, playermaxhealth;


	// Use this for initialization
	void Start () {
		hudtimer = 5;
		GameObject go = GameObject.FindGameObjectWithTag("HudHp");
		thehud = go;
	}
	
	// Update is called once per frame
	void Update () {
		hudtimer -=Time.deltaTime;
		if (hudtimer < 0)
			hudtimer = 0;
		if (hudtimer == 0)
			Destroy (thehud);
		zerohpcheck = (playercurrenthealth / playermaxhealth * 100 / 5);
		if(zerohpcheck == 0){
			Destroy (thehud);
		}
		else{
			displaygraphicfloat = (playercurrenthealth / playermaxhealth * 100 / 5) - 1;
			displaygraphic = (int)displaygraphicfloat;
			this.GetComponent<GUITexture>().texture = hudgraphics[displaygraphic];
		}
	}
}
