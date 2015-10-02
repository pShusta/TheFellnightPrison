using UnityEngine;
using System.Collections;

public class MenuHp : PlayerHealth {
	public Texture2D[] hudgraphics;
	private float displaygraphicfloat;
	private int displaygraphic;
	// Use this for initialization
	void Start () {
		displaygraphicfloat = (playercurrenthealth / playermaxhealth * 100 / 5) - 1;
		displaygraphic = (int)displaygraphicfloat;
		this.GetComponent<GUITexture>().texture = hudgraphics[displaygraphic];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
