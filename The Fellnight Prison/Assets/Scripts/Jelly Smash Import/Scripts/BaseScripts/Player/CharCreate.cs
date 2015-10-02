using UnityEngine;
using System.Collections;

public class CharCreate : MonoBehaviour {
	private bool nothing;
	private string _leveltoload = "";
	private string _thegame = "GameScreen";
	public Material redmat;
	public Material greenmat;
	public Material bluemat;
	public Material silvermat;
	public Material yellowmat;
	public GameObject FaceEquip, HatEquip, TopHat, Monocle;
	private GameObject go, PlayerHat, PlayerFace, PlayerJoint;
	
	// Use this for initialization
	void Start () {
		_leveltoload = _thegame;
		PlayerJoint = GameObject.FindGameObjectWithTag("playerjoint");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(PlayerPrefs.GetString("curmat"));	
	}
	void OnGUI() {
		
		if(GUI.Button (new Rect(50, 50, 50, 20), "Blue")){
			GetComponent<Renderer>().material = bluemat;
			PlayerPrefs.SetString("curmat", "bluemat");
		}
		if(GUI.Button (new Rect(50, 75, 50, 20), "Green")){
			GetComponent<Renderer>().material = greenmat;
			PlayerPrefs.SetString("curmat", "greenmat");
		}
		if(GUI.Button (new Rect(50, 100, 50, 20), "Red")){
			GetComponent<Renderer>().material = redmat;
			PlayerPrefs.SetString("curmat", "redmat");
		}
		if(GUI.Button (new Rect(50,125, 50, 20), "Silver")){
			GetComponent<Renderer>().material = silvermat;
			PlayerPrefs.SetString("curmat", "silvermat");
		}
		if(GUI.Button (new Rect(50,150, 50, 20), "Yellow")){
			GetComponent<Renderer>().material = yellowmat;
			PlayerPrefs.SetString("curmat", "yellowmat");
		}
		if(GUI.Button (new Rect(110,125, 70, 20), "TopHat")){
			PlayerHat = GameObject.FindGameObjectWithTag("hat");
			try{
				Destroy (PlayerHat);
			}
			catch{
			}
			go = Instantiate(TopHat, HatEquip.transform.position, HatEquip.transform.rotation) as GameObject;
			go.transform.parent = PlayerJoint.transform;
		}
		if(GUI.Button (new Rect(110,150, 70, 20), "Monocle")){
			GetComponent<Renderer>().material = yellowmat;
			PlayerPrefs.SetString("curmat", "yellowmat");
		}
		if(GUI.Button (new Rect(50,175, 50, 20), "Create")){
			Debug.Log("Playing");
			if(_leveltoload == ""){
				return;
			}
			if(Application.GetStreamProgressForLevel(_leveltoload) == 1){
				if(Application.CanStreamedLevelBeLoaded(_leveltoload)){
					Application.LoadLevel(_leveltoload);
				}
			}
		}
	}
}
