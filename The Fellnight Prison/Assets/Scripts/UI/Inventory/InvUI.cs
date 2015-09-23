using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InvUI : MonoBehaviour {
private UISlider _slider;
private List<string> _inventory;
private int _speed;
private float _value, _rot, _numItems;
private Transform _wheelTrans;

public UILabel _name, _desciption;
public GameObject _wheelObj;

	void Start(){
		_speed = 500;
		_slider = this.GetComponentInChildren<UISlider>();
		_slider.sliderValue = 0;
		_wheelTrans = _wheelObj.transform;
		_inventory = new List<string>();
		for(int I = 0; I < 25; I++){
			_inventory.Add("Item: " + I);
		}
	}
	void Update(){
		_numItems = _inventory.Count - 1;
		_rot = 22.5f * _slider.sliderValue * _numItems;
		//_wheelTrans.rotation = Quaternion.Euler(Mathf.Lerp(_wheelTrans.rotation.eulerAngles.x, _rot, Time.deltaTime), 0, 0);
		_wheelTrans.rotation = Quaternion.Euler(_rot, _wheelTrans.rotation.y, _wheelTrans.rotation.z);
		//_wheelTrans.rotation = Quaternion.Slerp(_wheelTrans.rotation, Quaternion.Euler(_rot, _wheelTrans.rotation.y, _wheelTrans.rotation.z), Time.deltaTime * 2);
		if(Input.GetMouseButtonUp(0)){
			_wheelTrans.rotation = Quaternion.Euler((float)Math.Floor(_rot / 22.5f) * 22.5f, _wheelTrans.rotation.y, _wheelTrans.rotation.z);
		}
		_value = Input.GetAxis("Mouse ScrollWheel");
		if(_value != 0){
			if(_value > 0)
				_slider.sliderValue -= 1/_numItems;
			else
				_slider.sliderValue += 1/_numItems;
			_value = 0;
		}
		/*
		if(_rotPool != 0){
			_time = Time.deltaTime;
			_change = _time * _speed;
			if(_change >= _rotPool){
				_change = _rotPool;
				_rotPool = 0;
			} else {
				_rotPool -= _change;
			}
			_thisMuch = _wheelTrans.rotation.eulerAngles.x + _change;
			while (_thisMuch > 360){
				_thisMuch -= 360;
			}
			//_wheelTrans.Rotate(new Vector3(_thisMuch, 0, 0));
			_wheelTrans.rotation = Quaternion.Slerp(_wheelTrans.rotation, Quaternion.Euler(_wheelTrans.rotation.x + _thisMuch, _wheelTrans.rotation.y, _wheelTrans.rotation.z), _time);
			//_wheelTrans.rotation = Quaternion.Euler ((_wheelTrans.rotation.eulerAngles.x + _change), _wheelTrans.rotation.eulerAngles.y, _wheelTrans.rotation.eulerAngles.z);
			//_wheelTrans.rotation = Quaternion.Slerp(_wheelTrans.rotation, Quaternion.Euler(_thisMuch, 0, 0), _time);
		}
		*/
	}








/* Legacy Script for depreciated prefab
private UISlider _slider;
private List<string> _inventory;
private List<GameObject> _panels, _items;
private Quaternion pos1, pos2, pos3, pos4, pos5;
private int _speed, _numItems, ticks;
private float thing;

public UILabel _name, _description;
public GameObject _invPanel1, _invPanel2, _invPanel3, _invPanel4, _invPanel5, _inventorySpawn;

	// Use this for initialization
	void Start () {
		pos1 = Quaternion.Euler(90, 0 , 0);
		pos2 = Quaternion.Euler(45, 0 , 0);
		pos3 = Quaternion.Euler(0, 0 , 0);
		pos4 = Quaternion.Euler(-45, 0 , 0);
		pos5 = Quaternion.Euler(-90, 0 , 0);
		_panels = new List<GameObject>();
		_panels.Add(_invPanel1);
		_panels.Add(_invPanel2);
		_panels.Add(_invPanel3);
		_panels.Add(_invPanel4);
		_panels.Add(_invPanel5);
		
		
		_speed = 5;
		_slider = this.GetComponentInChildren<UISlider>();
		_slider.sliderValue = 0;
		_inventory = new List<string>();
		for(int I = 0; I < 25; I++){
			_inventory.Add("Item: " + I);
		}
		_numItems = _inventory.Count;
		
		thing = 1f/(_numItems);
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_numItems != 0){
		
			ticks = (int)Math.Floor(_slider.sliderValue / thing);
			if(ticks >= _numItems){
				ticks = _numItems-1;
			}
			Debug.Log(ticks);
			for(int i = 0; i < 5; i++){
				if(ticks == 0){
					_panels[1].GetComponentInChildren<UILabel>().text = _inventory[ticks];
					_panels[2].GetComponentInChildren<UILabel>().text = _inventory[ticks+1];
				//}else if(ticks == _numItems){
				}else if(ticks == 1){
					_panels[1].GetComponentInChildren<UILabel>().text = _inventory[ticks-1];
					_panels[2].GetComponentInChildren<UILabel>().text = _inventory[ticks];
					_panels[3].GetComponentInChildren<UILabel>().text = _inventory[ticks+1];
				}else if(ticks == 2){
					_panels[4].GetComponentInChildren<UILabel>().text = _inventory[ticks+1];
				//}else if(ticks == _numItems-1){
				//	_panels[0].GetComponentInChildren<UILabel>().text = _inventory[ticks-2];
				//	_panels[1].GetComponentInChildren<UILabel>().text = _inventory[ticks-1];
				} else {
					_panels[(ticks-2 )%5].GetComponentInChildren<UILabel>().text = _inventory[ticks-3];
					try{
						_panels[(ticks+2 )%5].GetComponentInChildren<UILabel>().text = _inventory[ticks+1];
					} catch {
					}
				}
			}
			_name.text = _inventory[ticks];
			_description.text = "Description of " + _inventory[ticks];
			
			
			_panels[(ticks + 5) % 5].transform.localScale = new Vector3(1,1,1);
			_panels[(ticks + 2) % 5].transform.localScale = new Vector3(1,1,1);
			_panels[(ticks + 1) % 5].transform.localScale = new Vector3(1,1,1);
			_panels[(ticks + 4) % 5].transform.rotation = Quaternion.Slerp(_panels[(ticks + 4) % 5].transform.rotation, pos1, Time.deltaTime * _speed);
			_panels[(ticks + 5) % 5].transform.rotation = Quaternion.Slerp(_panels[(ticks + 5) % 5].transform.rotation, pos2, Time.deltaTime * _speed);
			_panels[(ticks + 1) % 5].transform.rotation = Quaternion.Slerp(_panels[(ticks + 1) % 5].transform.rotation, pos3, Time.deltaTime * _speed);
			_panels[(ticks + 2) % 5].transform.rotation = Quaternion.Slerp(_panels[(ticks + 2) % 5].transform.rotation, pos4, Time.deltaTime * _speed);
			_panels[(ticks + 3) % 5].transform.rotation = Quaternion.Slerp(_panels[(ticks + 3) % 5].transform.rotation, pos5, Time.deltaTime * _speed);
			_panels[(ticks + 4) % 5].transform.localScale = new Vector3(0,0,0);
			_panels[(ticks + 3) % 5].transform.localScale = new Vector3(0,0,0);
			if(ticks == 0){
				_panels[0].transform.localScale = new Vector3(0,0,0);
			}
			if(ticks == _numItems-1){
				_panels[_numItems%5+1].transform.localScale = new Vector3(0,0,0);
			}
		}	
	}
	
	public void TickUp(){
		_slider.sliderValue += 1f/(_numItems-1);
	}
	public void TickDown(){
		_slider.sliderValue -= 1f/(_numItems-1);
	}
	*/
	public void TickUp(){
		_slider.sliderValue += 1/_numItems;
	}
	public void TickDown(){
		_slider.sliderValue -= 1/_numItems;
	}
	public float GetInvCount(){
		return _numItems;
	}
}
