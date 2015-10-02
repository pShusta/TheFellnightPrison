using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
	private string _thegame = "JellySmash_Main";
	private string _settings = "JellySmash_Settings";
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {	
	}
	
	void OnGUI(){
		if(GUI.Button(new Rect(Screen.width / 2 - 65, Screen.height / 2, 130, 20), "Play")){
			if(Application.GetStreamProgressForLevel(_thegame) == 1){
				if(Application.CanStreamedLevelBeLoaded(_thegame)){
					Application.LoadLevel(_thegame);
				}
			}
		}
		if(GUI.Button(new Rect(Screen.width / 2 - 65, Screen.height / 2 + 35, 130, 20), "Controls")){
			if(Application.GetStreamProgressForLevel(_settings) == 1){
				if(Application.CanStreamedLevelBeLoaded(_settings)){
					Application.LoadLevel(_settings);
				}
			}
		}
		if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 70, 100, 20), "Quit")){
			Application.Quit();
		}
		
	}
	
}
