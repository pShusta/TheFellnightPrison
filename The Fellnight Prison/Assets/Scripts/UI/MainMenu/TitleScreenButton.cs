using UnityEngine;
using System.Collections;

public class TitleScreenButton : MonoBehaviour {
public GameObject Crate;
public GameObject Words;
public GameObject[] OtherCrates;
public GameObject[] OtherWords;
public bool solo;
public bool team;
public bool quit;
public float x, y;

private float Wait, x2;
private bool MoveOn = false;
private RectTransform Trans;

	// Use this for initialization
	void Start () {
		Wait = 0;
		Trans = this.GetComponent<RectTransform>();
		if(x < 0.3 ) {  
			x2 = ((((float)Screen.width / (float)Screen.height) - (float)1.1) / (float)0.937) * (float)0.18;
		} else if ( x > 0.6 ) { 
			x2 = ((((float)Screen.width / (float)Screen.height) - (float)1.1) / (float)0.937) * (float)-0.175;
			//Debug.Log ("> - " +(Screen.width / Screen.height / (float)1.77) * .039);
		} else {
			//x2 = 0;
		}
		Trans.position = new Vector3((Screen.width * (x + x2)), Screen.height * y, 0);
		Debug.Log ("x: " + Screen.width + " y: " + Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
		if(Wait > 0 && MoveOn == true){
			Wait -= Time.deltaTime;
			if(Wait <= 0){
				Pick ();
			}
		}
	}
	
	public void OnClick(){
		Crate.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		foreach(GameObject _words in OtherWords) { _words.SetActive(false); }
		foreach(GameObject _crate in OtherCrates){ _crate.GetComponent<Rigidbody>().useGravity = true; }
		Wait = 3;
		MoveOn = true;
	}
	
	
	void Pick(){
		if(quit){
			Quit();
		} else if (team) {
			Team();
		} else {
			Solo();
		}
	}
	
	void Solo(){
		Application.LoadLevel("Dungeon");
	}
	
	void Team(){
		Application.LoadLevel("Networking");
	}
	
	void Quit(){
		Application.Quit();
	}

}
