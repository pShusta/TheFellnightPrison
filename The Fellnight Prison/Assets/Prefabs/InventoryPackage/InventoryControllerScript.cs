using UnityEngine;
using System.Collections;

public class InventoryControllerScript : MonoBehaviour {

private GameObject _theInventory;
private bool _open;

public GameObject Controller;
public GameObject _inventoryMenu;

	// Use this for initialization
	void Start () {
		_open = false;
	}

    public void closeWindow()
    {
        try
        {
            Debug.Log("closeWindow Destroy");
            Destroy(_theInventory);
            Controller.GetComponent<Controller>().setCurMenu(null);
        }
        catch
        {
            Debug.Log("closeWindow Catch");
        }
        Debug.Log("closeWindow _open = false");
        _open = false;
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.I)){
            Debug.Log("Open Inventory");
			if(!_open){
                Debug.Log("Not Open Yet");
                GameObject curMenu = Controller.GetComponent<Controller>().getCurMenu();
                if (curMenu != null)
                {
                    Debug.Log("curMenu != null");
                    curMenu.SetActive(false);
                }
                Debug.Log("Instantiate Inv");
				_theInventory = Instantiate(_inventoryMenu);
                Controller.GetComponent<Controller>().setCurMenu(_theInventory);
				_theInventory.transform.SetParent(this.transform);
				_open = true;
			} else {
                Debug.Log("Inv Open");
				try {
                    Debug.Log("Destroy Inv");
					Destroy(_theInventory);
                    Controller.GetComponent<Controller>().setCurMenu(null);
				} catch {
                    Debug.Log("Inv Destroy Catch");
				}
				_open = false;
			}
		}
	}
}
