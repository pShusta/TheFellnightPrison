using UnityEngine;
using System.Collections;

public class TitleScreenAnim : MonoBehaviour {
public GameObject[] Anchors;
public GameObject Text;

private float _time;
private float _newTime;
private bool _animation = true;
	// Use this for initialization
	void Start () {
		_time = Time.time;
	}

	void Update(){
		if(_animation){
			_newTime = Time.time;
			if((_newTime - _time) > 1 && (_newTime - _time) < 3){
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Anchors[0].transform.localRotation, (Time.deltaTime * 2));
				this.transform.position = Vector3.Slerp(this.transform.position, Anchors[0].transform.position, (Time.deltaTime * 2));
			}
			if((_newTime - _time) > 3 && (_newTime - _time) < 7){
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Anchors[1].transform.localRotation, (Time.deltaTime * 2));
				this.transform.position = Vector3.Slerp(this.transform.position, Anchors[1].transform.position, (Time.deltaTime * 2));
			}
			if((_newTime - _time) > 6){
				_animation = false;
				NGUITools.SetActive(Text, true);
			}
		}
	}
}
