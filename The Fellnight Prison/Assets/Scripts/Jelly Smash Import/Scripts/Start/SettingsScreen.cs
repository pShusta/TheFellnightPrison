using UnityEngine;
using System.Collections;

public class SettingsScreen : MonoBehaviour {
	private string _thegame = "JellySmash_Main";
	
	// Use this for initialization
	void Start () {
		this.GetComponent<GUIText>().text = "WASD: Move \nF: Attack \nM: Menu \nE: Generic Event\n(ex. Accept Quest) \nEsc: Quit";
	
	}
	
	// Update is called once per frame
	void OnGUI(){
		if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 35, 100, 20), "Play")){
			if(Application.GetStreamProgressForLevel(_thegame) == 1){
				if(Application.CanStreamedLevelBeLoaded(_thegame)){
					Application.LoadLevel(_thegame);
				}
			}
		}
		if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 70, 100, 20), "Quit")){
			Application.Quit();
		}
	}
}
