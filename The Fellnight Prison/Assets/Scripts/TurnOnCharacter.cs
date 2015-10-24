using UnityEngine;
using System.Collections;

public class TurnOnCharacter : MonoBehaviour {

    private GameObject Menu;
    public MonoBehaviour script;
	// Use this for initialization
	void Start () {
        if (this.gameObject.GetComponent<PhotonView>().isMine)
        {
            this.gameObject.GetComponent<Animator>().enabled = true;
            this.gameObject.GetComponentInChildren<Camera>().enabled = true;
            this.gameObject.GetComponentInChildren<AudioListener>().enabled = true;
            script.enabled = true;
            Menu = GameObject.FindGameObjectWithTag("PlayerMenu");
            Menu.SetActive(false);
        }
	}

    void Update()
    {
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
