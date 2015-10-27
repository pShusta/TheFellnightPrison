using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

    private PhotonPlayer inviter;
    public GameObject curMenu;
    public GameObject mainMenu, inventory, socialMenu, partyMessageBox;
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
        if (Input.GetKeyUp(KeyCode.S) && clear)
        {
            Debug.Log("Called Social menu");
            if (socialMenu.activeSelf)
            {
                lockLook = false;
                curMenu.SetActive(false);
            }
            else
            {
                curMenu.SetActive(false);
                curMenu = socialMenu;
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

    public void invitePlayerToParty(PhotonPlayer _inviter)
    {
        partyMessageBox.SetActive(true);
        partyMessageBox.GetComponentInChildren<Text>().text = _inviter.name + " has invited you to a party.";
        inviter = _inviter;
    }

    public void partyInviteAccept()
    {
        partyMessageBox.SetActive(false);
        Debug.Log("Inviter: " + inviter);
        socialMenu.SetActive(true);
        curMenu = socialMenu;
        socialMenu.GetComponent<SocialScreen>().joinParty(inviter);
    }

    public void partyInviteDecline()
    {
        partyMessageBox.SetActive(false);
    }
}
