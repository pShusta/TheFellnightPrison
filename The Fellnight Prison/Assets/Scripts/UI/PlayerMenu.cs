using UnityEngine;
using System.Collections;

public class PlayerMenu : MonoBehaviour {

    public GameObject Menu;
    public PhotonView theView;
    public bool isMine;
    public MonoBehaviour script;
    //public Component script;

	// Use this for initialization
	void Start () {
        Debug.Log("Running PlayerMenu");
        
        if (!this.gameObject.GetComponent<PhotonView>().isMine)
        {
            this.enabled = false;
        }
        else
        {
            Menu = GameObject.FindGameObjectWithTag("PlayerMenu");
            Menu.SetActive(false);
            this.gameObject.GetComponent<CharacterController>().enabled = true;
            script.enabled = true;
            this.gameObject.GetComponentInChildren<Camera>().enabled = true;
            this.gameObject.GetComponentInChildren<AudioListener>().enabled = true;
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
