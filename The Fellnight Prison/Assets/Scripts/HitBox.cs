using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {

    private float MaxDamage;
    public bool blocked, mob, limb;
    public GameObject MobHealth, damageResponse;

	void Start () {
        if (!PhotonNetwork.isMasterClient)
        {
            this.enabled = false;
        }
	}
	
	void Update () {
        if (limb)
        {
            MaxDamage = MobHealth.GetComponent<MobHealth>().MaxHp * (float)0.4;
        }
        else
        {
            MaxDamage = MaxDamage = MobHealth.GetComponent<MobHealth>().MaxHp;
        }
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
            
            damageResponse = (GameObject)PhotonNetwork.Instantiate("powEffect", this.gameObject.transform.position, Quaternion.identity, 0);
            //damageResponse.gameObject.transform.parent = this.gameObject.transform;
                
        }
        else
        {
            MobHealth.GetComponent<MasterPlayerHealth>().takeDamage(_value);
        }
    }
}