using UnityEngine;
using System.Collections;

public class PlayerMenu : MonoBehaviour {

    public GameObject Menu, Controller;
    public PhotonView theView;
    public bool isMine;
    public MonoBehaviour script;

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

	void Update () {
	}
}
