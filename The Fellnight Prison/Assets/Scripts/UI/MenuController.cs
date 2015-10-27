using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    public GameObject curMenu;
    public GameObject mainMenu, inventory;
    public bool lockLook, clear;
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
                lockLook = false;
                curMenu.SetActive(false);
            }
            else
            {
                curMenu.SetActive(false);
                curMenu = inventory;
                curMenu.SetActive(true);
                lockLook = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape) && clear)
        {
            Debug.Log("Called Main Menu");
            if (mainMenu.activeSelf)
            {
                lockLook = false;
                curMenu.SetActive(false);
            }
            else
            {
                curMenu.SetActive(false);
                curMenu = mainMenu;
                curMenu.SetActive(true);
                lockLook = true;
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
