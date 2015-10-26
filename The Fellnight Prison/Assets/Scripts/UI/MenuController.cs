using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    private bool clear;
    public GameObject curMenu;
    public GameObject mainMenu, inventory;
	// Use this for initialization
	void Start () {
        clear = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.I) && clear)
        {
            Debug.Log("Called Inventory");
            if (inventory.activeSelf)
            {
                curMenu.SetActive(false);
            }
            else
            {
                curMenu.SetActive(false);
                curMenu = inventory;
                curMenu.SetActive(true);
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape) && clear)
        {
            Debug.Log("Called Main Menu");
            if (mainMenu.activeSelf)
            {
                curMenu.SetActive(false);
            }
            else
            {
                curMenu.SetActive(false);
                curMenu = mainMenu;
                curMenu.SetActive(true);
            }
        }
	}

    public void setClear(bool _value)
    {
        clear = _value;
    }

    public void clearFalse()
    {
        clear = false;
    }

    public void clearTrue()
    {
        clear = true;
    }
}
