using UnityEngine;
using System.Collections;

public class MobHealth : MonoBehaviour {

    public int MaxHp, expValue;
    public float CurHp;
    private float timer;
    private bool run = false;
    private GameObject tempPow;
    public GameObject generator = null;

	// Use this for initialization
	void Start () {
        CurHp = MaxHp;
	}
	
	// Update is called once per frame
	void Update () {
        if (run)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                run = false;
                PhotonNetwork.Destroy(tempPow);
            }
        }
        if (CurHp <= 0)
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (generator != null)
                {
                    generator.GetComponent<MobGenerator>().RemoveMob(this.gameObject);
                }

                PhotonNetwork.Destroy(this.gameObject);
                int temp = Random.Range(0, 100);
                bool _temp;
                if (temp < 5)
                {
                    _temp = true;
                }
                else
                {
                    _temp = false;
                }
                //int money
                GameObject.FindWithTag("GameController").GetComponent<PhotonView>().RPC("mobKill", PhotonTargets.MasterClient, _temp, expValue);
            }
        }
	}

    public void takeDamage(float _value)
    {
        CurHp -= _value;
        tempPow = (GameObject)PhotonNetwork.Instantiate("powEffect", this.gameObject.transform.position, Quaternion.identity, 0);
        timer = 2;
        run = true;
    }

    public void setGenerator(GameObject _generator)
    {
        generator = _generator;
    }
}
