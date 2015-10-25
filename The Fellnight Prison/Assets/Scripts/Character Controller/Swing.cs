using UnityEngine;
using System.Collections;

public class Swing : MonoBehaviour
{
    public Camera cam;
    public Animator anim;
    private float inputX;
    private float inputY;
    private float Run;
    private float inputH;
    private GameObject Controller;
    private bool okay;

    private bool isRunning = false;

    public GameObject Weapon;
    private BoxCollider sword;

    public Animation clip;

    public float moveSpeed = 5f;
    public float MS = 5f;
    public float VS = 3f;

    float VR = 0;
    public float UDR = 60f;

    public GameObject playerCamra;
    public GameObject bone;

    // Use this for initialization
    void Start()
    {
        okay = true;
        Controller = GameObject.FindGameObjectWithTag("GameController");
        anim = GetComponent<Animator>();
        sword = Weapon.GetComponent<BoxCollider>();
        sword.enabled = false;
        clip = GetComponent<Animation>();
        //Time.timeScale = 0.5F;

    }

    public int frame = 0;
    public int runFrames = 200;


    public int inputIndex = 0;
    public int runIndex = 30;
    IEnumerator xInput()
    {
        while (inputIndex < runIndex)
        {
            if (inputX != 0)
            {
                anim.SetTrigger("Attack");
                this.gameObject.GetComponent<PhotonView>().RPC("setTrigger", PhotonTargets.All, "Attack");
                //startCollider();
                //StartCoroutine("colliderOn");
                inputIndex = 30;
            }
            inputIndex++;
            yield return null;


        }

        inputIndex = 0;

    }

    /*
    public void startCollider()
    {
        frame = 0;
        sword.enabled = true;
        MS = .5f;
        VS = 1;
    }
    void turnOffCollider()
    {
        anim.ResetTrigger("Attack");
        sword.enabled = false;
        frame = -1;
        MS = 5;
        VS = 3;
    }
     */

    // Update is called once per frame
    void Update()
    {
        //if (frame >= 0)
        //{
        //    frame++;
        //    if (frame >= runFrames)
        //    {
        //        turnOffCollider();
        //    }
        //}

        if (Controller.GetComponent<Controller>().curMenu != null && okay)
        {
            okay = false;
            //PlayerToon.GetComponent<MonoBehaviour>()
            //kill mouse look
            //kill attack
        }
        else if (Controller.GetComponent<Controller>().curMenu == null && !okay)
        {
            okay = true;
            //enable mouse look
            //enable attack
        }

        playerCamra.transform.position = bone.transform.position;

        if (okay)
        {
            //Handles camera and player rotation
            float rotLR = Input.GetAxis("Mouse X") * MS;
            transform.Rotate(0, rotLR, 0);
            VR -= Input.GetAxis("Mouse Y") * VS;
            VR = Mathf.Clamp(VR, -UDR, UDR);
            Camera.main.transform.localRotation = Quaternion.Euler(VR, 0, 0);
        }


        //Gets the mouse direction and saves it to inputX/inputY
        inputY = Input.mouseScrollDelta.y;
        inputX = Input.GetAxis("Mouse X");
        //Then sets the animation paramiter float to that value
        anim.SetFloat("posX", inputX);
        anim.SetFloat("Ydirection", inputY * -1);


        //Same as above but for movement
        Run = Input.GetAxis("Vertical");
        inputH = Input.GetAxis("Horizontal");
        //Run is forward and input H is sideways
        anim.SetFloat("Run", Run);
        anim.SetFloat("inputH", inputH);


        //Hold down leftshit to sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            anim.SetBool("isRunning", true);
            //Debug.Log("shift being held down");
        }
        else
        {
            isRunning = false;
            anim.SetBool("isRunning", false);
        }


        //press right mouse button to block and let go to stop blocking
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetBool("isBlocking", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("isBlocking", false);
        }

        if (okay)
        {
            //Handles player attacking
            //if player clicks left mouse button and moves in a direction an attack will occur from that direction
            if (Input.GetButtonDown("Fire1"))
            {


                //anim.SetTrigger("Attack");
                StartCoroutine("xInput");


            }
            if (Input.mouseScrollDelta.y > 0)
            {
                // if (inputH != 0 || inputY != 0)
                //{
                anim.SetTrigger("Attack");
                this.gameObject.GetComponent<PhotonView>().RPC("setTrigger", PhotonTargets.All, "Attack");
                //startCollider();
                //StartCoroutine("colliderOn");

                //anim.ResetTrigger("Attack");

                // }
            }
        }

        /*
        if (Input.GetButtonDown("Fire1") && (Input.GetAxis("Mouse X") == -1))
        {
                //play animation for left move
                anim.Play("Attack2", -1, 0f);
        }
        if (Input.GetButtonDown("Fire1") && (Input.GetAxis("Mouse X") == 1))
        {
                //play animation for right move
                anim.Play("Attack1", -1, 0f);
        }
        */

    }
}