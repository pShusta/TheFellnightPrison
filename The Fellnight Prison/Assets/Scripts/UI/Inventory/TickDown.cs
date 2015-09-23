using UnityEngine;
using System.Collections;

public class TickDown : MonoBehaviour {

	// Use this for initialization
	void OnClick () {
		this.GetComponentInParent<InvUI>().TickDown();
	}

	

}
