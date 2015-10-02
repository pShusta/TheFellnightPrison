using UnityEngine;
using System.Collections;

public class InstanEquip : MonoBehaviour {
	public GameObject Monocle, TopHat, FaceEquip, HatEquip;
	private GameObject go, go2, PlayerJoint;
	// Use this for initialization
	void Start () {
		PlayerJoint = GameObject.FindGameObjectWithTag("playerjoint");
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.E)){
			go = Instantiate(Monocle, FaceEquip.transform.position, FaceEquip.transform.rotation) as GameObject;
			go.transform.parent = PlayerJoint.transform;
			go2 = Instantiate(TopHat, HatEquip.transform.position, HatEquip.transform.rotation) as GameObject;
			go2.transform.parent = PlayerJoint.transform;
		}
		if(Input.GetKeyUp(KeyCode.R)){
			Destroy(go);
			Destroy(go2);
		}
	}
}