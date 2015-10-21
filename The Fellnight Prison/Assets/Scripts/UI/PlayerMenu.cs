using UnityEngine;
using System.Collections;

public class PlayerMenu : MonoBehaviour {

    public GameObject Menu;

	// Use this for initialization
	void Start () {
        Menu = GameObject.FindGameObjectWithTag("PlayerMenu");
        Menu.SetActive(false);
        if (this.GetComponent<PhotonView>().isMine)
        {
            this.GetComponent<CharacterController>().enabled = true;
            this.GetComponent<AudioSource>().enabled = true;
            //this.GetComponent<FirstPersonController>().enabled = true;
        }
        else
        {
            this.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Menu.active)
            {
                Menu.SetActive(false);
            }
            else
            {
                Menu.SetActive(true);
            }
        }
	}
}
