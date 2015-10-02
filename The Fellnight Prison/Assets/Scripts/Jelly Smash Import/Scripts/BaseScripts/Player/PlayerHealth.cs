using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public static float playermaxhealth = 100.0f;
	public static float playercurrenthealth = 100.0f;
	public GameObject hud;
	private GameObject targethud;
	public static float hudtimer = 0;
	private bool hudison, exitmenu, menuopen;
	public GameObject hudmana;
	private GameObject targetmana;
	private GameObject targethp;
	public GameObject hudhp, menu;
	private float scalex, scaley, positionx, positiony, killcooldown;
	public static float portaltimer, killcount;
	private GUITexture settexture;

	// Use this for initialization
	void Start () {
		hudison = false;
		exitmenu = false;
		scalex = (float)Screen.width / 1273 * 2 / 5;
		scaley = (float)Screen.height / 1420 * 2 / 5;
		positionx = (float)Screen.height / 3854 * 2 / 5;
		positiony = (float)Screen.width / 2929 * 2 / 5;
		PlayerPrefs.SetInt ("numberofquests", 0);
		killcount = 0;
		menuopen = false;
		Messenger<int>.AddListener("got kill", killcounter);
	}
	
	// Update is called once per frame
	void Update () {
		if (killcooldown >= .1f)
			killcooldown -= Time.deltaTime;
		if(Input.GetKeyUp(KeyCode.Escape)){
			exitmenu = true;
		}
		if(Input.GetKeyUp(KeyCode.E)){
			Messenger.Broadcast("genericevent");
		}
		if(Input.GetKeyUp(KeyCode.M)){
			if(!menuopen){
				Instantiate(menu, new Vector3(0,0, 0), Quaternion.identity);
				GameObject setmenu = GameObject.FindGameObjectWithTag("Menu");
				setmenu.transform.position = new Vector3(0,0,0);
				settexture = setmenu.GetComponent<GUITexture>();
				settexture.pixelInset = new Rect(Screen.width/2-96, Screen.height/2-96, 192, 192);
				menuopen = true;
			}
			else{
				GameObject menutokill = GameObject.FindGameObjectWithTag("Menu");
				Destroy(menutokill);
				menuopen = false;
			}
		}
		if(playercurrenthealth == 0){
			transform.gameObject.tag = "Dead";
		}
		if(hudtimer > 0)
			hudtimer -= Time.deltaTime;
		if (hudtimer < 0)
			hudtimer = 0;
		if (hudtimer == 0 && hudison) {
			hudison = false;
		}
		if(portaltimer > 0)
			portaltimer -= Time.deltaTime;
		if(portaltimer < 0)
			portaltimer = 0;
	}
	
	void OnGUI() {
		if(playercurrenthealth == 0)
			GUI.Box (new Rect(Screen.width / 2 - 50, Screen.height / 2 - 10, 100, 20), "Your dead");
		if(exitmenu){
			if(GUI.Button (new Rect(50, 50, 50, 15), "exit"))
				Application.Quit();
			if(GUI.Button (new Rect(50, 70, 50, 15), "cancel"))
				exitmenu = false;
		}
	}
	
	public void adjusthealth(int adjustment){
		playercurrenthealth += adjustment;
		if(playercurrenthealth < 0)
			playercurrenthealth = 0;
		if(playermaxhealth < 1)
			playermaxhealth = 1;
		if(playercurrenthealth > playermaxhealth)
			playercurrenthealth = playermaxhealth;

		if (adjustment != 0) {
			hudtimer = 5;
			if (hudison == false) {
				Instantiate (hud, new Vector3 (0.1f, 0.23f, 0), Quaternion.identity);
				Instantiate (hudmana, new Vector3 (0.1f, 0.23f, 0), Quaternion.identity);
				Instantiate (hudhp, new Vector3 (0.1f, 0.23f, 0), Quaternion.identity);
				GameObject gomana = GameObject.FindGameObjectWithTag("HudMana");
				targetmana = gomana;
				GameObject gohp = GameObject.FindGameObjectWithTag("HudHp");
				targethp = gohp;
				GameObject gohud = GameObject.FindGameObjectWithTag("Hud");
				targethud = gohud;
				targethud.transform.localScale = new Vector3(scaley, scalex, 1);
				targetmana.transform.localScale = new Vector3(scaley, scalex, 1);
				targethp.transform.localScale = new Vector3(scaley, scalex, 1);
				targethp.transform.position = new Vector3(positionx, positiony, 1);
				targetmana.transform.position = new Vector3(positionx, positiony, 1);
				targethud.transform.position = new Vector3(positionx, positiony, 1);
			}
			hudison = true;
		}
	}
	
	private void killcounter(int heal){
		if(killcooldown < .3f){
			killcount++;
			killcooldown = .5f;
		}
	}
}
