using UnityEngine;
using System.Collections;

public class PlayerMat : MonoBehaviour {
	public Material Blue_Jelly, Red_Jelly, Yellow_Jelly, Silver_Jelly, Pink_Jelly, Green_Jelly;
	private float ranmat;
	
		// Use this for initialization
	void Start () {
		ranmat = Random.Range(0f, 5f);
		getmat((int)ranmat);
	}
	
	public void getmat(int materialint){
		switch(materialint){
		case (0):
			GetComponent<Renderer>().material = Blue_Jelly;
			return;
		case (1):
			GetComponent<Renderer>().material = Red_Jelly;
			return;
		case (2):
			GetComponent<Renderer>().material = Yellow_Jelly;
			return;
		case (3):
			GetComponent<Renderer>().material = Silver_Jelly;
			return;
		case (4):
			GetComponent<Renderer>().material = Pink_Jelly;
			return;
		case (5):
			GetComponent<Renderer>().material = Green_Jelly;
			return;
		}
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
