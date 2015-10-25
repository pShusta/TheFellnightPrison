using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    //public GameObject weapon;
    //public Player me;
    private float curHp, maxHp;
    private GameObject healthbar, curHealth, maxHealth;
	// Use this for initialization
	void Start () {
        if (this.gameObject.GetComponent<PhotonView>().isMine && !PhotonNetwork.isMasterClient)
        {
            maxHp = 1;
            curHp = 1;
            healthbar = GameObject.FindGameObjectWithTag("HealthPanel");
            curHealth = GameObject.FindGameObjectWithTag("CurHp");
            maxHealth = GameObject.FindGameObjectWithTag("MaxHp");
            maxHealth.GetComponent<Text>().text = maxHp.ToString();
            //sword = weapon.GetComponent<Box
        }
        else
        {
            this.enabled = false;
        }
        
	}

    public float frame = 0;
    public int runFrames = 200;
    public float MS = 5f;
    public float VS = 3f;
    
    public BoxCollider sword;
    public Animator anim;

    public void startCollider()
    {
        frame = Time.time;
        sword.enabled = true;
        MS = .5f;
        VS = 1;
    }
    void turnOffCollider()
    {
        anim.ResetTrigger("Attack");
        sword.enabled = false;
        frame = -1;
        MS = 5;
        VS = 3;
    }

	// Update is called once per frame
	void Update () {
        if (frame >= 0)
        {
            if (Time.time - frame > 5)
            {
                turnOffCollider();
            }
        }

        curHealth.GetComponent<Text>().text = curHp.ToString();
        if (curHp <= 0)
        {
            Debug.Log("You're Dead!");
        }
	}

    [PunRPC]
    void PlayerSetHealth(float MaxHealth)
    {
        maxHp = MaxHealth;
        curHp = maxHp;
        maxHealth.GetComponent<Text>().text = maxHp.ToString();
    }

    [PunRPC]
    void PlayerTakeDamage(float _value)
    {
        curHp -= _value;
    }
}
