using UnityEngine;
using System.Collections;

public class Portals : JellyPlayerHealth {
	public GameObject portal;
	private GameObject player;
	public int killcost;
	private float timer;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(JellyPlayerHealth.killcount >= killcost && this.GetComponent<ParticleSystem>().emissionRate == 0)
            this.GetComponent<JellyPlayerHealth>().emissionRate = 50;
	}
	
	void OnTriggerEnter(Collider other){
        timer = JellyPlayerHealth.portaltimer;
		if (other.transform.CompareTag("Player") && JellyPlayerHealth.killcount >= killcost && timer == 0){
			player.transform.position = portal.transform.position;
			player.transform.rotation = portal.transform.rotation;
            JellyPlayerHealth.portaltimer = 2;
		}
	}
}
