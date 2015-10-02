using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
private Rect btn1, btn2, btn3, btn4;
private GUITexture settexture;
public GameObject menuhp, menumana;
	// Use this for initialization
	void Start () {
		Debug.Log("Screen.width: " + Screen.width);
		Debug.Log("Screen.height: " + Screen.height);
		btn1 = new Rect(Screen.width/2 + 15, Screen.height/2 - 60, 40, 40);
		btn2 = new Rect(Screen.width/2 + 55, Screen.height/2 - 25, 40, 40);
		btn3 = new Rect(Screen.width/2 - 67, Screen.height/2 + 15, 40, 40);
		btn4 = new Rect(Screen.width/2 - 27, Screen.height/2 + 50, 40, 40);
		
		Instantiate(menuhp, new Vector3(0,0, 0), Quaternion.identity);
		GameObject setmenuhp = GameObject.FindGameObjectWithTag("MenuHp");
		setmenuhp.transform.position = new Vector3(0,0,0);
		settexture = setmenuhp.GetComponent<GUITexture>();
		settexture.pixelInset = new Rect(Screen.width/2-96, Screen.height/2-96, 192, 192);
		setmenuhp.transform.parent = this.transform;
		
		Instantiate(menumana, new Vector3(0,0, 0), Quaternion.identity);
		GameObject setmenumana = GameObject.FindGameObjectWithTag("MenuMana");
		setmenumana.transform.position = new Vector3(0,0,0);
		settexture = setmenumana.GetComponent<GUITexture>();
		settexture.pixelInset = new Rect(Screen.width/2-96, Screen.height/2-96, 192, 192);
		setmenumana.transform.parent = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnGUI() {
		if (Event.current.type == EventType.MouseUp && btn1.Contains(Event.current.mousePosition))
		{
			Debug.Log("Hit Player");
		}
		if (Event.current.type == EventType.MouseUp && btn2.Contains(Event.current.mousePosition))
		{
			Debug.Log("Hit Inventory");
		}
		if (Event.current.type == EventType.MouseUp && btn3.Contains(Event.current.mousePosition))
		{
			Debug.Log("Hit Social");
		}
		if (Event.current.type == EventType.MouseUp && btn4.Contains(Event.current.mousePosition))
		{
			Debug.Log("Hit Settings");
		}
	}
}
