using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NewRandRoom : MonoBehaviour
{

    public int numToSpawn;
    private int numSpawned = 0;

    public Transform TempNode;

    public GameObject[] SceneNodes;
    //public static GameObject[] myObjects;
    public GameObject[] roomBounds;
    public GameObject[] Rooms;

    public GameObject[] DoorNodes;
    public GameObject[] WallNodes;
    public GameObject[] DeleteNodes;

    void SpawnRandomObject()
    {
        numSpawned++;


        roomBounds = GameObject.FindGameObjectsWithTag("Bounds");
        Debug.Log(roomBounds.Length);



        //spawns item in list position between 0 and the number of objects in myObjects
        int whichItem = Random.Range(0, Rooms.Length);
        //Puts all spawned objects with the Node tag in the SceneNodes Array
        //Then chooses random Node in array and saves it to ParentNode
        SceneNodes = GameObject.FindGameObjectsWithTag("Node");
        int Index_SceneNode = Random.Range(0, SceneNodes.Length);
        GameObject ParentNode = SceneNodes[Index_SceneNode];

        //Keep this below the SceneNodes.FindObjectsWithTag, otherwise it'll pick up the new rooms Nodes
        //GameObject myObj = Instantiate(Rooms[whichItem], new Vector3(6, 6, 6), Quaternion.identity) as GameObject;

        GameObject myObj = PhotonNetwork.Instantiate(Rooms[whichItem].name, new Vector3(6, 6, 6), Quaternion.identity, 0) as GameObject;

        //Finds all of the Ray Cast Nodes in the new object
        var RayNodes = myObj.transform.Cast<Transform>().Where(c => c.gameObject.tag == "Ray Cast Node").ToArray();
        //Finds all of the Nodes in the new object

        var RoomNodes = myObj.transform.Cast<Transform>().Where(c => c.gameObject.tag == "Node").ToArray();
        int Index_RoomNode = Random.Range(0, RoomNodes.Length);
        Transform ChildNode = RoomNodes[Index_RoomNode];

        //For room building the positive X axis acts as the top side while looking down at the room
        var Top_Side = ChildNode.transform.Cast<Transform>().Where(c => c.gameObject.tag == "Top Node").ToArray();
        var Bottom_Side = ChildNode.transform.Cast<Transform>().Where(c => c.gameObject.tag == "Bottom Node").ToArray();
        var Left_Side = ChildNode.transform.Cast<Transform>().Where(c => c.gameObject.tag == "Left Node").ToArray();
        var Right_Side = ChildNode.transform.Cast<Transform>().Where(c => c.gameObject.tag == "Right Node").ToArray();

        //Debug.Log(Top_Side.Length);

        myObj.transform.rotation = ParentNode.transform.parent.rotation;
        //Determines if the ChildNode is on the top bottom left or right
        //Then determines the position of the ParentNode
        //and then rotates the rooms so they line up
        if (ChildNode.localPosition.x > 0)
        {
            //Debug.Log ("Child Node is Node 1");
            if (ParentNode.transform.localPosition.x > 0)
                myObj.transform.Rotate(0, 180, 0); //Debug.Log("It runs this");
            if (ParentNode.transform.localPosition.x < 0)
                myObj.transform.Rotate(0, 0, 0);
            if (ParentNode.transform.localPosition.z < 0)
                myObj.transform.Rotate(0, -90, 0);
            if (ParentNode.transform.localPosition.z > 0)
                myObj.transform.Rotate(0, 90, 0);
        }
        if (ChildNode.localPosition.x < 0)
        {
            //Debug.Log ("Child Node is Node 3");
            if (ParentNode.transform.localPosition.x > 0)
                myObj.transform.Rotate(0, 0, 0);
            if (ParentNode.transform.localPosition.x < 0)
                myObj.transform.Rotate(0, 180, 0);
            if (ParentNode.transform.localPosition.z < 0)
                myObj.transform.Rotate(0, 90, 0);
            if (ParentNode.transform.localPosition.z > 0)
                myObj.transform.Rotate(0, -90, 0);
        }
        if (ChildNode.localPosition.z < 0)
        {
            //Debug.Log ("Child Node is Node 2");
            if (ParentNode.transform.localPosition.x > 0)
                myObj.transform.Rotate(0, 90, 0);
            if (ParentNode.transform.localPosition.x < 0)
                myObj.transform.Rotate(0, -90, 0);
            if (ParentNode.transform.localPosition.z < 0)
                myObj.transform.Rotate(0, 180, 0);
            if (ParentNode.transform.localPosition.z > 0)
                myObj.transform.Rotate(0, 0, 0);
        }
        if (ChildNode.localPosition.z > 0)
        {
            //Debug.Log ("Child Node is Node 4");
            if (ParentNode.transform.localPosition.x > 0)
                myObj.transform.Rotate(0, -90, 0);
            if (ParentNode.transform.localPosition.x < 0)
                myObj.transform.Rotate(0, 90, 0);
            if (ParentNode.transform.localPosition.z < 0)
                myObj.transform.Rotate(0, 0, 0);
            if (ParentNode.transform.localPosition.z > 0)
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
        TempNode.transform.position = ParentNode.transform.position;


        
        var newBounds = myObj.GetComponent<Collider>();

        int i = 0;
        bool checkspace = true;

        while (i < roomBounds.Length && checkspace == true)
        {
            

            var oldBounds = roomBounds[i].GetComponent<Collider>();
            var nb = newBounds.bounds;
            var ob = oldBounds.bounds;
           // Debug.Log(nb);

            if (nb.Intersects(ob))
            {
                Debug.Log("Spawned Item interceted with other item");
                ParentNode.gameObject.tag = "Blocked Node";
                Destroy(myObj);
                numSpawned--;
                checkspace = false;
            }
            i++;
        }
        if (checkspace == true)
        {
            Debug.Log("Spawned succsessful");

            TempNode.transform.position = ParentNode.transform.position;
            //TempNode.DetachChildren();
            myObj.transform.parent = ParentNode.transform;
            ParentNode.gameObject.tag = "Used Node";
            ChildNode.gameObject.tag = "Used Node";
        }
        

    }




    void CloseDoors()
    {
        int i = 0;

        DoorNodes = GameObject.FindGameObjectsWithTag("Door");
        WallNodes = GameObject.FindGameObjectsWithTag("Wall");


        while (i < WallNodes.Length)
        {
            Transform ChildWall = WallNodes[i].transform;

            if (ChildWall.parent.tag == "Used Node")
            {
                WallNodes[i].gameObject.tag = "Delete Me";
            }

            i++;

        }
        i = 0;

        while (i < DoorNodes.Length)
        {
            Transform ChildDoor = DoorNodes[i].transform;

            if (ChildDoor.parent.tag == "Node")
            {
                DoorNodes[i].gameObject.tag = "Delete Me";
            }

            i++;


        }

        DeleteNodes = GameObject.FindGameObjectsWithTag("Delete Me");
        for (int p = 0; p < DeleteNodes.Length; p++)
        {
            Destroy(DeleteNodes[p].gameObject);
        }



    }

    /*
    bool checkspace = true;

        TempNode.transform.position = ParentNode.transform.position + Vector3.up;
        int iRay = 0;
        while (iRay < RayNodes.Length)
        {
            //Debug.Log("place in ray cast nodes" + RayNodes[iRay]);
            if (iRay < RayNodes.Length * 0.5f)
            {
                //Debug.DrawRay (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward * myObj.transform.localScale.z, Color.blue, 10000);
                if (Physics.Raycast(RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward, myObj.transform.localScale.z))
                {
                    //Debug.Log ("RayCast NOde Hit a thing!!!!!!!");
                    Destroy(myObj);
                    numSpawned--;
                    iRay = RayNodes.Length;
                    checkspace = false;
                }

            }
            else
            {
                //Debug.DrawRay (RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward * myObj.transform.localScale.x, Color.red, 10000);
                if (Physics.Raycast(RayNodes[iRay].transform.position, RayNodes[iRay].transform.forward, myObj.transform.localScale.x))
                {
                    //Debug.Log ("RayCast NOde Hit a thing!!!!!!!");
                    Destroy(myObj);
                    numSpawned--;
                    iRay = RayNodes.Length;
                    checkspace = false;
                }
            }
            iRay++;
        }
        if (checkspace == true)
        {

            TempNode.transform.position = ParentNode.transform.position;
            //TempNode.DetachChildren();
            myObj.transform.parent = ParentNode.transform;
            ParentNode.gameObject.tag = "Used Node";
            ChildNode.gameObject.tag = "Used Node";
        }

    }

    */

    void Start()
    {
        //myObjects = Resources.LoadAll<GameObject>("Rooms");
        //Debug.Log(myObjects.Length);

    }

    public void launch()
    {
        Debug.Log("launch()");
        while (numToSpawn > numSpawned)
        {
            SpawnRandomObject();
        }
        CloseDoors();
    }

    void Update()
    {
        
        if (numToSpawn > numSpawned)
        {
            SpawnRandomObject();
        }
        if (numToSpawn <= numSpawned)
        {
            CloseDoors();
            this.gameObject.GetComponent<MainController>().FinishGenerator();
            this.enabled = false;
        }
        
    }

}
