using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour {
	public GameObject target;
	public float attacktimer = 0;
	public float cooldown = 2;
	private Transform myTransform;
	public Transform publictrans;

	void Start () {
		//The attack timer and cooldown are used so you can only attack so fast
		//change the cooldown to change attackspeed
		myTransform = transform;
		publictrans = transform;
	}
	
	void Update () {
		//the if structure is used as an is this enemy alive?  the other option from enemy is dead
		if(myTransform.gameObject.tag == "Enemy"){
			//searches for a possible target
			GameObject go = GameObject.FindGameObjectWithTag("Player");
			target = go;
			//the next two if structures are used to tick down the time till it can attack again
			if(attacktimer > 0)
			    attacktimer -= Time.deltaTime;
			if(attacktimer < 0)
			    attacktimer = 0;
		    //when the timer is 0 it will attack again
			if(attacktimer == 0){
				attack();
				attacktimer = cooldown;
			}
		}
	}
	
	private void attack() {
		//finds the distance from the enemy to the target
		float distance = Vector3.Distance(target.transform.position,  transform.position);
		//finds the direction of the target in comparison to your facing
		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot(dir, transform.forward);
		//if the target is infront of the enemy and they are close enough then the mob will attack
		if(distance < 2 && direction > 0 && attacktimer == 0) {
			PlayerHealth ph = (PlayerHealth)target.GetComponent("PlayerHealth");
			ph.adjusthealth(-10);
			GetComponent<Animation>().Blend("attack", 1.0f);
		}
	}
}
