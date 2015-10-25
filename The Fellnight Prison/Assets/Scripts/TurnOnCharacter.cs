using UnityEngine;
using System.Collections;

public class TurnOnCharacter : MonoBehaviour {

    private bool okay;
    private GameObject Menu, Controller, healthbar;
    public MonoBehaviour script;
	// Use this for initialization
	void Start () {
        okay = true;
        if (this.gameObject.GetComponent<PhotonView>().isMine)
        {
            Controller = GameObject.FindGameObjectWithTag("GameController");
            //this.gameObject.GetComponent<Animator>().enabled = true;
            this.gameObject.GetComponentInChildren<Camera>().enabled = true;
            this.gameObject.GetComponentInChildren<AudioListener>().enabled = true;
            script.enabled = true;
            Menu = GameObject.FindGameObjectWithTag("PlayerMenu");
            Menu.SetActive(false);
            //healthbar = GameObject.FindGameObjectWithTag("HealthPanel");
            //healthbar.SetActive(true);
        }
        else
        {
            this.enabled = false;
        }
	}

    public void closeWindow(){
        Menu.SetActive(false);
        Controller.GetComponent<Controller>().setCurMenu(null);
    }

    void Update()
    {
        

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Menu.active)
            {
                Menu.SetActive(false);
                Controller.GetComponent<Controller>().setCurMenu(null);
            }
            else
            {
                GameObject curMenu = Controller.GetComponent<Controller>().getCurMenu();
                if (curMenu != null)
                {
                    GameObject.FindGameObjectWithTag("InventoryController").GetComponent<InventoryControllerScript>().closeWindow();
                }
                Menu.SetActive(true);
                Controller.GetComponent<Controller>().setCurMenu(Menu);
            }
        }
    }

}
