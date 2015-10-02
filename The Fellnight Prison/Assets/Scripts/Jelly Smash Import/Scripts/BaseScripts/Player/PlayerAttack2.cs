using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerAttack2 : MonoBehaviour {
private List<Transform> targets;
private List<float> stats;
private Transform mytransform, target;
private float attrng, attarc, attacktimer, cooldown, attdamage;
private bool weapchanging;
public string weaponchange;

	void Start () {
		targets = new List<Transform>();
		GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject enemy in go)
			targets.Add(enemy.transform);
		mytransform = transform;
		attarc = 0;
		attrng = 0;
		cooldown = 1;
		attdamage = 10;
		getweapstats();
		attacktimer = 0;
		weapchanging = false;
	}
	

	void Update () {
		Sorttargets();
		if(attacktimer > 0)
			attacktimer -= Time.deltaTime;
		if(attacktimer < 0)
			attacktimer = 0;
		if(Input.GetKeyUp(KeyCode.F) && attacktimer == 0){
			Attack();
			attacktimer = cooldown;
		}
		if(Input.GetKeyUp (KeyCode.Q)){
			resetstats();
			changeweap();
		}
	}
	
	private void Sorttargets(){
		targets.Sort(delegate(Transform t1, Transform t2){ 
			return Vector3.Distance (t1.position, mytransform.position).CompareTo(Vector3.Distance (t2.position, mytransform.position));
		});
	}
	private void resetstats(){
		attarc = 0;
		attrng = 0;
		attdamage = 10;
		
	}
	private void getweapstats(){
		string currweap = PlayerPrefs.GetString("currweap");
		stats = new List<float> (PickWeap.pickweap(currweap));
		attarc += stats[0];
		attrng += stats[1];
		attdamage += stats[2];
		cooldown += stats[3];
	}
	private void Attack(){
		target = targets[0];
		
		float distance = Vector3.Distance (target.transform.position, transform.position);
		
		Vector3 dir = (target.transform.position - transform.position).normalized;
		
		float direction = Vector3.Dot(dir, transform.forward);
		if(distance < attrng && direction < attarc && transform.gameObject.tag == "Player") {
			EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
			eh.adjusthealth(-(int)attdamage);
			GetComponent<Animation>().Blend("attack", 1.0f);
		}
	}	
	private void changeweap(){
		weapchanging = true;
	}
	void OnGUI(){
		if(weapchanging == true){
			GUI.SetNextControlName("changeweapon");
			weaponchange = GUI.TextArea(new Rect(50, 50, 100, 20), weaponchange, 20);
			GUI.FocusControl("changeweapon");
			if(Event.current.keyCode == KeyCode.Return){
				GUI.FocusControl(null);
				PlayerPrefs.SetString("currweap", weaponchange);
				getweapstats();
				weaponchange = "";
				weapchanging = false;
			}
		}
	}
}