using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InvUI : MonoBehaviour {
private UISlider _slider;
public List<BaseItem> _inventory;
public double curItem;
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
        _inventory = new List<BaseItem>();
        updateInv();
	}

    void OnEnable()
    {
        updateInv();
    }

    public void updateInv()
    {
        _inventory = new List<BaseItem>();
        try
        {
            foreach (Weapon _w in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().player.InvWeapons)
            {
                _inventory.Add(_w);
            }
            foreach (CraftingMaterial _w in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().player.InvMaterials)
            {
                _inventory.Add(_w);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error thrown in inventory script: " + e.Message);
        }
        if (_inventory.Count == 0)
        {
            _wheelObj.SetActive(false);
        }
        else
        {
            _wheelObj.SetActive(true);
        }
    }

	void Update(){
        if (_wheelObj.activeSelf)
        {
            _numItems = _inventory.Count - 1;
            _rot = 22.5f * _slider.sliderValue * _numItems;
            //_slider.sliderValue = Math.Round()
            _wheelTrans.rotation = Quaternion.Euler(_rot, _wheelTrans.rotation.y, _wheelTrans.rotation.z);
            if (Input.GetMouseButtonUp(0))
            {
                _wheelTrans.rotation = Quaternion.Euler((float)Math.Round((_rot / 22.5f), 0, MidpointRounding.AwayFromZero) * 22.5f, _wheelTrans.rotation.y, _wheelTrans.rotation.z);
            }
            _value = Input.GetAxis("Mouse ScrollWheel");
            if (_value != 0)
            {
                if (_value > 0)
                    _slider.sliderValue -= 1 / _numItems;
                else
                    _slider.sliderValue += 1 / _numItems;
                _value = 0;
            }
            curItem = _inventory[(int)(Math.Round((_slider.sliderValue * _numItems), 0, MidpointRounding.ToEven))].Id;
            _name.text = _inventory[(int)(Math.Round((_slider.sliderValue * _numItems), 0, MidpointRounding.ToEven))].Name;
            _desciption.text = _inventory[(int)(Math.Round((_slider.sliderValue * _numItems ), 0, MidpointRounding.ToEven))].Name;
        }
        else
        {
            _name.text = "Inventory Empty";
            _desciption.text = "You have no items";
        }
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
