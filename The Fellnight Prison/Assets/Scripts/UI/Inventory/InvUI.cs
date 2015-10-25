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
        foreach (Weapon _w in GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>()._player.InvWeapons)
        {
            _inventory.Add(_w.GetName());
        }
        foreach (CraftingMaterial _w in GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>()._player.InvMaterials)
        {
            _inventory.Add(_w.GetName());
        }
	}

	void Update(){
		_numItems = _inventory.Count - 1;
		_rot = 22.5f * _slider.sliderValue * _numItems;
		_wheelTrans.rotation = Quaternion.Euler(_rot, _wheelTrans.rotation.y, _wheelTrans.rotation.z);
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
        Debug.Log("Slider Value: " + _slider.sliderValue);
	}

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
