using UnityEngine;
using System.Collections;

public class MakeMap : MonoBehaviour {

	public Transform P1;
	public Transform CH1;
	
	public Transform P2;
	public Transform CH2;

	public Transform TempNode;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Fuck my ass
	void Update () {
		if (Input.GetKeyDown ("r")) {
			TempNode.position = CH1.position;
			P1.transform.parent = TempNode.transform;
			TempNode.position = CH2.position;
			TempNode.rotation = CH2.rotation;
			TempNode.DetachChildren();

		}

	}
}
/*


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RandRoom : MonoBehaviour {
	
	public int numToSpawn = 1;
	public Transform TempNode;
	
	
	private Transform objectArray;
	private Transform objectList;
	private Transform objectToMoveTo;
	private Vector3 offset;
	private int index;
	
	public List<GameObject> Children;
	
	//define a list
	public static List <GameObject> myObjects = new List<GameObject>();
	
	//I used this to keep track of the number of objects I spawned in the scene.
	private int numSpawned = 0;
	
	public static int numToSpawned = 4;
	
	void Start()
	{
		
		//Important note: place your prefabs folder(or levels or whatever) 
		//in a folder called "Resources" like this "Assets/Resources/Prefabs"
		Object[] subListObjects = Resources.LoadAll("Tiles", typeof(GameObject));
		
		//This may be sloppy (I've only been programing for a short time) 
		//It works though :) 
		foreach (GameObject subListObject in subListObjects) 
		{    
			GameObject lo = (GameObject)subListObject;
			
			myObjects.Add(lo);
		}
	}
	
	void PlaceObject()
	{
		
		
	}
	
	
	
	
	void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("RoomCol"))
		{
			Debug.Log("Collides with rooms");
		}
	} 
	
	
	private int ch;
	private int pa;
	public GameObject[] SceneNodes;
	
	
	
	void SpawnRandomObject() 
	{    
		
		
		//spawns item in list position between 0 and 100
		int whichItem = Random.Range (0,9);
		
		
		numSpawned++;
		
		
		SceneNodes = GameObject.FindGameObjectsWithTag("Node");
		int Index_SceneNode = Random.Range (0,SceneNodes.Length);
		GameObject ParentNode = SceneNodes [Index_SceneNode];
		Debug.Log ("Open Nodes in scene: " + SceneNodes.Length);
		
		
		//Keep this below the SceneNodes declaration, otherwise it'll pick up the rooms Nodes
		GameObject myObj = Instantiate (myObjects [whichItem], new Vector3 (6,6,6), Quaternion.identity) as GameObject;
		//
		var RayNodes = myObj.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Ray Cast Node").ToArray();
		
		var RoomNodes = myObj.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Node").ToArray();
		Debug.Log ("The Children in that object" + RoomNodes.Length + myObj);
		int Index_RoomNode = Random.Range (0, RoomNodes.Length);
		Transform ChildNode = RoomNodes [Index_RoomNode];
		
		myObj.transform.rotation = ParentNode.transform.parent.rotation;
		
		if (ChildNode.localPosition.x > 0) {
			Debug.Log ("Child Node is Node 1");
			int ch = 1;
			if (ParentNode.transform.localPosition.x > 0)
				myObj.transform.Rotate(0, 180, 0); Debug.Log("It runs this");
			if (ParentNode.transform.localPosition.x < 0)
				myObj.transform.Rotate(0, 0, 0);
			if (ParentNode.transform.localPosition.z < 0)
				myObj.transform.Rotate(0, -90, 0);
			if (ParentNode.transform.localPosition.z > 0)
				myObj.transform.Rotate(0, 90, 0);
		}
		if (ChildNode.localPosition.x < 0) {
			Debug.Log ("Child Node is Node 3");
			int ch = 3;
			if (ParentNode.transform.localPosition.x > 0)
				myObj.transform.Rotate(0, 0, 0);
			if (ParentNode.transform.localPosition.x < 0)
				myObj.transform.Rotate(0, 180, 0);
			if (ParentNode.transform.localPosition.z < 0)
				myObj.transform.Rotate(0, 90, 0);
			if (ParentNode.transform.localPosition.z > 0)
				myObj.transform.Rotate(0, -90, 0);
		}
		if (ChildNode.localPosition.z < 0) {
			Debug.Log ("Child Node is Node 2");
			int ch = 2;
			if (ParentNode.transform.localPosition.x > 0)
				myObj.transform.Rotate(0, 90, 0);
			if (ParentNode.transform.localPosition.x < 0)
				myObj.transform.Rotate(0, -90, 0);
			if (ParentNode.transform.localPosition.z < 0)
				myObj.transform.Rotate(0, 180, 0);
			if (ParentNode.transform.localPosition.z > 0)
				myObj.transform.Rotate(0, 0, 0);
		}
		if (ChildNode.localPosition.z > 0) {
			Debug.Log ("Child Node is Node 4");
			int ch = 4;
			if (ParentNode.transform.localPosition.x > 0)
				myObj.transform.Rotate(0, -90, 0);
			if (ParentNode.transform.localPosition.x < 0)
				myObj.transform.Rotate(0, 90, 0);
			if (ParentNode.transform.localPosition.z < 0)
				myObj.transform.Rotate(0, 0, 0);
			if (ParentNode.transform.localPosition.z > 0)
				myObj.transform.Rotate(0, 180, 0);
		}
		
		
		
		
		
		
		TempNode.position = ChildNode.transform.position;
		myObj.transform.parent = TempNode.transform;
		
		
		bool checkspace = true;
		
		
		Debug.Log ("This should say that there are five ray cast Nodes " + RayNodes.Length);
		Debug.Log ("Name of Node " + RayNodes[0]);
		
		TempNode.transform.position = ParentNode.transform.position + Vector3.up;
		int iRay = 0;
		while(iRay < RayNodes.Length){
			Debug.Log("place in ray cast nodes" + RayNodes[iRay]);
			if(iRay < RayNodes.Length * 0.5f){
				//Debug.DrawRay (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward * myObj.transform.localScale.z, Color.blue, 10000);
				if (Physics.Raycast (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward, myObj.transform.localScale.z))
				{
					Debug.Log ("RayCast NOde Hit a thing!!!!!!!");
					Destroy (myObj);
					numSpawned--;
					iRay = RayNodes.Length;
					checkspace = false;
				}
				
			}
			else 
			{
				//Debug.DrawRay (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward * myObj.transform.localScale.x, Color.red, 10000);
				if (Physics.Raycast (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward, myObj.transform.localScale.x)){
					Debug.Log ("RayCast NOde Hit a thing!!!!!!!");
					Destroy (myObj);
					numSpawned--;
					iRay = RayNodes.Length;
					checkspace = false;
				}
			}
			iRay++;
		}
		if (checkspace == true) {
			
			TempNode.transform.position = ParentNode.transform.position;
			//TempNode.DetachChildren();
			myObj.transform.parent = ParentNode.transform;
			ParentNode.gameObject.tag = "Used Node";
			ChildNode.gameObject.tag = "Used Node";
		}
		//TempNode.position = arrayOfChildren[0];
		//myObj.transform.parent = TempNode.transform;
		//TempNode.position = target.position;
		
		//Debug.Log ("The Game Object" + myObj);
	}
	
	
	
	void Update() 
	{/*
		if (Input.GetKeyDown("f"))
			SpawnRandomObject ();

		if (numToSpawn > numSpawned) 
		{
			//where your instantiated object spawns from
			transform.position = new Vector3(0, 0, 0);
			
			SpawnRandomObject ();
		}
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	public int numToSpawn = 5;

	//define an Array
	public static GameObject[] myObjects;
	
	//I used this to keep track of the number of objects I spawned in the scene.
	public static int numSpawned = 0;
	
	void Start()
	{
		//Important note: place your prefabs folder(or levels or whatever) 
		//in a folder called "Resources" like this "Assets/Resources/Prefabs"
		myObjects = Resources.LoadAll<GameObject>("Tiles");
		
	}
	
	
	
	void SpawnRandomObject() 
	{    
		//spawns item in array position between 0 and 100
		int whichItem = Random.Range (0, 4);

		nodes = GameObject.FindGameObjectsWithTag("Node");
		Debug.Log (nodes.Length);



		var arrayOfChildren = myObj.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Node").ToArray();
		Debug.Log ("The Children in that object" + arrayOfChildren[0] + myObj);

		
		GameObject myObj = Instantiate (myObjects [whichItem]) as GameObject;
		
		numSpawned++;
		
		myObj.transform.position = transform.position;
	}
	
	void Update() 
	{
		if (numToSpawn > numSpawned) 
		{
			//where your instantiated object spawns from
			transform.position = new Vector3(0, 0, 0);
			
			SpawnRandomObject ();
		}
	}

}
*/