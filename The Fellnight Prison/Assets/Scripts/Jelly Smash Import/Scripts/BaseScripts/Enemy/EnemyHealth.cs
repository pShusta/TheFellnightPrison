using UnityEngine;
using System.Collections;

public class EnemyHealth : EnemyAI {
	public int MaxHealth = 40;
	public int CurrentHealth = 40;
	private float healthbarlength;
	public float healthtimer;
	private bool _healthbar;
	private Transform myTransform;
	private GameObject targethud, gohud;
	public GameObject hud;
	public Texture2D[] hudgraphics;
	private float displaygraphicfloat;
	private int displaygraphic;
	public float zerohpcheck;	
	
	void Awake() {
		mytransform = transform;
		
	}
	// Use this for initialization
	void Start () {
		healthbarlength = Screen.width / 2;
		_healthbar = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(healthtimer > 0)
			healthtimer -= Time.deltaTime;
		if(healthtimer < 0)
			healthtimer = 0;
		if(healthtimer == 0 && _healthbar){
			_healthbar = false;
			Destroy(gohud);
		}
		if(CurrentHealth == 0){
			if(transform.gameObject.tag == "Enemy"){
				GetComponent<Animation>()["death"].wrapMode = WrapMode.Once;
				GetComponent<Animation>().Play("death");
				Messenger<int>.Broadcast("got kill", 30);
				healthtimer = 0;
				_healthbar = false;
				Destroy(gohud);
			}
			transform.gameObject.tag = "Dead";
		}
		if (_healthbar){
			zerohpcheck = (float)CurrentHealth / (float)MaxHealth * 20;
			displaygraphicfloat = zerohpcheck - 1;
			displaygraphic = (int)displaygraphicfloat;
			if(displaygraphic >= 0)
				gohud.GetComponent<GUITexture>().texture = hudgraphics[displaygraphic];
			if(displaygraphic < 0)
				Destroy(gohud);
		}
	}
	
	void OnGUI() {
	}
	
	public float healthcheck(){
		float enemyhealth = ((float)this.CurrentHealth / (float)this.MaxHealth * 100 / 5);
		return enemyhealth;
	}
	
	public void adjusthealth(int adjustment){
		CurrentHealth += adjustment;
		if(CurrentHealth < 0)
			CurrentHealth = 0;
		if(MaxHealth < 1)
			MaxHealth = 1;
		if(CurrentHealth > MaxHealth)
			CurrentHealth = MaxHealth;
		healthtimer = 5;
		if (_healthbar == false) {
			Instantiate (hud, new Vector3 (0.1f, 0.23f, 0), Quaternion.identity);
			gohud = GameObject.FindGameObjectWithTag("EnemyHealthGui");
			targethud = gohud;
			targethud.transform.GetComponent<GUITexture>().pixelInset = new Rect(20, Screen.height - 14, 100, -10);
			targethud.transform.position = new Vector3(0,0,1);
			targethud.transform.localScale = new Vector3(0,0,1);
			_healthbar = true;
		}
	}
}