using UnityEngine;
using System.Collections;

public class TurnOnCharacter : MonoBehaviour {

    private bool okay;
    private GameObject Menu, Controller, healthbar;
    public MonoBehaviour script;
    public bool GM;
	// Use this for initialization
	void Start () {
        if (this.gameObject.GetComponent<PhotonView>().isMine)
        {
            this.gameObject.GetComponentInChildren<Camera>().enabled = true;
            this.gameObject.GetComponentInChildren<AudioListener>().enabled = true;
            script.enabled = true;
        }
        else
        {
            this.enabled = false;
        }
	}

    void Update()
    {
    }

}
