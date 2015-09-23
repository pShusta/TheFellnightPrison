using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RandRoom : MonoBehaviour {

	public int numToSpawn;
	private int numSpawned = 0;
	
	private bool _spawned = false;

	public Transform TempNode;

	public GameObject Globals;

	public GameObject[] SceneNodes;
	public static GameObject[] myObjects;

	void Awake(){
		if(!PhotonNetwork.connected){
			Globals.GetComponent<GlobalFunctions>().PhotonConnect();
		}
	}

	void Start()
	{		
		myObjects = Resources.LoadAll<GameObject>("Tiles");
        Debug.Log(myObjects.Length);
	}

	void SpawnRandomObject() 
	{    
		numSpawned++;

		//spawns item in list position between 0 and the number of objects in myObjects
		int whichItem = Random.Range (0, myObjects.Length);
		
		//Puts all spawned objects with the Node tag in the SceneNodes Array
		//Then chooses random Node in array and saves it to ParentNode
		SceneNodes = GameObject.FindGameObjectsWithTag("Node");
		int Index_SceneNode = Random.Range (0,SceneNodes.Length);
		GameObject ParentNode = SceneNodes [Index_SceneNode];

        //Keep this below the SceneNodes.FindObjectsWithTag, otherwise it'll pick up the new rooms Nodes
        GameObject myObj = PhotonNetwork.Instantiate (myObjects [whichItem].name, new Vector3 (6,6,6), Quaternion.identity, 0) as GameObject;
		//Finds all of the Ray Cast Nodes in the new object
		var RayNodes = myObj.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Ray Cast Node").ToArray();
		//Finds all of the Nodes in the new object

		var RoomNodes = myObj.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Node").ToArray();
		int Index_RoomNode = Random.Range (0, RoomNodes.Length);
		Transform ChildNode = RoomNodes [Index_RoomNode];
			
		//For room building the positive X axis acts as the top side while looking down at the room
		var Top_Side = ChildNode.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Top Node").ToArray();
		var Bottom_Side = ChildNode.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Bottom Node").ToArray();
		var Left_Side = ChildNode.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Left Node").ToArray();
		var Right_Side = ChildNode.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Right Node").ToArray();

		var PaTop_Side = ParentNode.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Top Node").ToArray();
		var PaBottom_Side = ParentNode.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Bottom Node").ToArray();
		var PaLeft_Side = ParentNode.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Left Node").ToArray();
		var PaRight_Side = ParentNode.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "Right Node").ToArray();

		
		Debug.Log (Top_Side.Length);

		myObj.transform.rotation = ParentNode.transform.parent.rotation;
		//Determines if the ChildNode is on the top bottom left or right
		//Then determines the position of the ParentNode
		//and then rotates the rooms so they line up
		if (Top_Side.Length > 0) {
			//Debug.Log ("Child Node is Node 1");
			if (PaTop_Side.Length > 0)
				myObj.transform.Rotate(0, 180, 0); Debug.Log("It runs this");
			if (PaBottom_Side.Length > 0)
				myObj.transform.Rotate(0, 0, 0);
			if (PaRight_Side.Length > 0)
				myObj.transform.Rotate(0, -90, 0);
			if (PaLeft_Side.Length > 0)
				myObj.transform.Rotate(0, 90, 0);
		}
		if (Bottom_Side.Length > 0) {
			//Debug.Log ("Child Node is Node 3");
			if (PaTop_Side.Length > 0)
				myObj.transform.Rotate(0, 0, 0);
			if (PaBottom_Side.Length > 0)
				myObj.transform.Rotate(0, 180, 0);
			if (PaRight_Side.Length > 0)
				myObj.transform.Rotate(0, 90, 0);
			if (PaLeft_Side.Length > 0)
				myObj.transform.Rotate(0, -90, 0);
		}
		if (Right_Side.Length < 0) {
			//Debug.Log ("Child Node is Node 2");
			if (PaTop_Side.Length > 0)
				myObj.transform.Rotate(0, 90, 0);
			if (PaBottom_Side.Length > 0)
				myObj.transform.Rotate(0, -90, 0);
			if (PaRight_Side.Length > 0)
				myObj.transform.Rotate(0, 180, 0);
			if (PaLeft_Side.Length > 0)
				myObj.transform.Rotate(0, 0, 0);
		}
		if (Left_Side.Length > 0) {
			//Debug.Log ("Child Node is Node 4");
			if (PaTop_Side.Length > 0)
				myObj.transform.Rotate(0, -90, 0);
			if (PaBottom_Side.Length > 0)
				myObj.transform.Rotate(0, 90, 0);
			if (PaRight_Side.Length > 0)
				myObj.transform.Rotate(0, 0, 0);
			if (PaLeft_Side.Length > 0)
				myObj.transform.Rotate(0, 180, 0);
		}
		//Note: This only works if the Node's localPositions are offset on one axis
		//However it could be simple enough to add something like...
		//if(ChildNode.localPosition.z < 0 && ChildNode.localPosition < 0)
			//rotate myObj to align with botton left corner

		//Set TempNode to same position as ChildNode
		//Set TempNode as objects parent so the room moves with the ChildNodes position acting as the center
		TempNode.position = ChildNode.transform.position;
		myObj.transform.parent = TempNode.transform;


		bool checkspace = true;
	
		TempNode.transform.position = ParentNode.transform.position + Vector3.up;
		int iRay = 0;
		while(iRay < RayNodes.Length){
			//Debug.Log("place in ray cast nodes" + RayNodes[iRay]);
			if(iRay < RayNodes.Length * 0.5f){
				//Debug.DrawRay (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward * myObj.transform.localScale.x, Color.blue, 10000);
				if (Physics.Raycast (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward, myObj.transform.localScale.x - 1f))
				{
					//Debug.Log ("RayCast NOde Hit a thing!!!!!!!");
					Destroy (myObj);
					numSpawned--;
					iRay = RayNodes.Length;
					checkspace = false;
				}

			}
			else 
			{
				//Debug.DrawRay (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward * myObj.transform.localScale.z, Color.red, 10000);
				if (Physics.Raycast (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward, myObj.transform.localScale.z - 1f)){
				//Debug.Log ("RayCast NOde Hit a thing!!!!!!!");
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

	}


	void Update() 
	{
        //Debug.Log("runs this");

        if (PhotonNetwork.isMasterClient){
			if (numToSpawn > numSpawned) 
			{
				SpawnRandomObject ();
			} else if ( !Globals.GetComponent<GlobalFunctions>().CheckReady() ) { Globals.GetComponent<GlobalFunctions>().SetReady(true); }
		}
		if(Globals.GetComponent<GlobalFunctions>().CheckReady() && _spawned == false) { 
			Globals.GetComponent<GlobalFunctions>().SpawnPlayer();
			_spawned = true;
		}
		
	}

}
