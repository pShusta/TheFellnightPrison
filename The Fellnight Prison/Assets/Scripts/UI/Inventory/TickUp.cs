using UnityEngine;
using System.Collections;

public class TickUp : MonoBehaviour {

	// Use this for initialization
	void OnClick () {
		this.GetComponentInParent<InvUI>().TickUp();
	}
}
