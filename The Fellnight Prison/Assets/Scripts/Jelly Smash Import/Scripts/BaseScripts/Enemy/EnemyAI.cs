using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public Transform target;
	public int movespeed = 1;
	public int rotationspeed = 1;
	public float enemysightrange = 10f;
	public float enemyhearingrange = 5f;
	private float distance2;
	private bool notdead;
	
	public Transform mytransform;
	
	void Awake() {
		mytransform = transform;
		
	}

	// Use this for initialization
	void Start () {
		if(mytransform.gameObject.tag == "Enemy"){
			GameObject go = GameObject.FindGameObjectWithTag("Player");
			target = go.transform;
			notdead = true;
		}
	}
	
	void Update(){
		distance2 = Vector3.Distance (target.transform.position, mytransform.position);
		watchforplayer();
		listenforplayer();
		if(mytransform.gameObject.tag == "Dead")
			notdead = false;
	}
	
	void watchforplayer(){
		Vector3 dir = (target.transform.position - mytransform.position).normalized;
		float direction = Vector3.Dot(dir, mytransform.forward);
		if(distance2 < enemysightrange && direction > .1){
			PlayerDetected();
		}
	}
	
	void listenforplayer(){
		if(distance2 < enemyhearingrange){
			PlayerDetected();
		}
	}
	
	void PlayerDetected() {
		if(mytransform.gameObject.tag == "Enemy" && notdead){
		    float distance = Vector3.Distance (target.transform.position, mytransform.position);

		    mytransform.rotation = Quaternion.Slerp(mytransform.rotation, Quaternion.LookRotation(target.position - mytransform.position), rotationspeed * Time.deltaTime);
		
		    if(distance > 1.5f)
			    mytransform.position += mytransform.forward * movespeed * Time.deltaTime;
		}
	
	}
}