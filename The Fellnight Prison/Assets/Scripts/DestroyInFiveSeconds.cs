using UnityEngine;
using System.Collections;

public class DestroyInFiveSeconds : MonoBehaviour {

    private float timer;
	// Use this for initialization
	void Start () {
        timer = 1;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
	}
}
