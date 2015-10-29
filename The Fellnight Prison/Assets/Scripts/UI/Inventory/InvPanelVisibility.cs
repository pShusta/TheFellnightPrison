using UnityEngine;
using System.Collections;
using System;

public class InvPanelVisibility : MonoBehaviour {
public float _numItems, _rot;
private Transform _wheelTrans;
private UIPanel _panel;

public UISlider _slider;
public int _panelNum;
public GameObject InvMain;

	void Start(){
		_wheelTrans = this.transform.parent;
		_panel = this.GetComponent<UIPanel>();
	}

    float ConvertBase(float _num, int _base)
    {
        while (_num >= _base)
        {
            _num -= _base;
        }
        if (_num < 0)
        {
            _num += _base;
        }
        return _num;
    }
    void BaseEnable()
    {
        if (ConvertBase(_rot, 16) < 13 && ConvertBase(_rot, 16) > 2)
        {
            if (_panelNum >= (ConvertBase((_rot - 3), 16)) && _panelNum <= (ConvertBase((_rot + 3), 16)))
            {
                _panel.enabled = true;
                float _num = (_rot - _panelNum) % 16;
                //Debug.Log("_slider.sliderValue * _numItems = " + (int)( Math.Round( (_slider.sliderValue * _numItems - _num), 0, MidpointRounding.ToEven)));
                _panel.GetComponentInChildren<UILabel>().text = InvMain.GetComponent<InvUI>()._inventory[(int)( Math.Round( (_slider.sliderValue * _numItems - _num), 0, MidpointRounding.ToEven))].Name;
            }
            else
            {
                _panel.enabled = false;
            }
        }
        else
        {
            if (_panelNum >= (ConvertBase((_rot - 3), 16)) || _panelNum <= (ConvertBase((_rot + 3), 16)))
            {
                _panel.enabled = true;
                float _num = (_rot - _panelNum) % 16;
                //Debug.Log("_slider.sliderValue * _numItems = " + (int)( Math.Round( (_slider.sliderValue * _numItems - _num), 0, MidpointRounding.ToEven)));
                _panel.GetComponentInChildren<UILabel>().text = InvMain.GetComponent<InvUI>()._inventory[(int)( Math.Round( (_slider.sliderValue * _numItems - _num), 0, MidpointRounding.ToEven))].Name;
            }
            else
            {
                _panel.enabled = false;
            }
        }
    }
	void Update () {
		_numItems = this.transform.parent.parent.GetComponent<InvUI>().GetInvCount();
		_rot = (float)Math.Floor((16f * (float)Math.Floor(_slider.sliderValue * _numItems)) / 16f);
        //Debug.Log("_rot: " + _rot);
		if((_rot < 3 && _panelNum > 12) || (_rot > _numItems - 3 && _panelNum > ConvertBase(_numItems, 16))){
			_panel.enabled = false;
		} else {
			BaseEnable();
		}
	}
}
