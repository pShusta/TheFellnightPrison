using UnityEngine;
using System.Collections;

public class MobHealth : MonoBehaviour {

    public int MaxHp;
    public float CurHp;

	// Use this for initialization
	void Start () {
        CurHp = MaxHp;
	}
	
	// Update is called once per frame
	void Update () {
        if (CurHp <= 0)
        {
            //dead
        }
	}

    public void takeDamage(float _value)
    {
        CurHp -= _value;
    }
}
