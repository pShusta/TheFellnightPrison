using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {

    public int MaxDamage;
    public bool blocked, mob;
    public GameObject MobHealth;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void takeDamage(float _value)
    {
        if (blocked)
        {
            _value = _value * (float)0.05;
        }
        if (_value > MaxDamage)
        {
            _value = MaxDamage;
        }
        if (mob)
        {
            MobHealth.GetComponent<MobHealth>().takeDamage(_value);
        }
    }
}