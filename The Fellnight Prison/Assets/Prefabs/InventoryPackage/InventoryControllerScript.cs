using UnityEngine;
using System.Collections;

public class InventoryControllerScript : MonoBehaviour {
private GameObject _theInventory;
private bool _open;

public GameObject _inventoryMenu;

	// Use this for initialization
	void Start () {
		_open = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.I)){
			if(!_open){
				_theInventory = Instantiate(_inventoryMenu);
				_theInventory.transform.SetParent(this.transform);
				_open = true;
			} else {
				try {
					Destroy(_theInventory);
				} catch {
				}
				_open = false;
			}
		}
	}
}
