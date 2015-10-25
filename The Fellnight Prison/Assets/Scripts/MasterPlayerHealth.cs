using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MasterPlayerHealth : MonoBehaviour
{

    //public GameObject weapon;
    public Player me;
    //private GameObject healthbar, curHealth, maxHealth;
    // Use this for initialization
    void Start()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            this.enabled = false;
        }
        else
        {
            foreach(Player _p in GameObject.FindGameObjectWithTag("GameController").GetComponent<Database>().Players){
                if(_p.Username == this.gameObject.GetComponent<PhotonView>().owner.name){
                    Debug.Log("Found Owner");
                    me = _p;
                    break;
                }
            }
            Debug.Log("Setting Health for: " + this.gameObject.GetComponent<PhotonView>().owner.name);
            this.gameObject.GetComponent<PhotonView>().RPC("PlayerSetHealth", this.gameObject.GetComponent<PhotonView>().owner, (float)me.MaxHp);
        }
    }

    public int frame = 0;
    public int runFrames = 25;
    public float MS = 5f;
    public float VS = 3f;
    
    public BoxCollider sword;
    public Animator anim;

    public void startCollider()
    {
        frame = 0;
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
    void Update()
    {
        if (frame >= 0)
        {
            frame++;
            if (frame >= runFrames)
            {
                turnOffCollider();
            }
        }

        if (me.CurHp <= 0)
        {
            Debug.Log("You're Dead!");
        }
    }

    public void setMe(Player _player)
    {
        me = _player;
    }

    public void takeDamage(float _value)
    {
        Debug.Log("MasterPlayerHealth.takeDamage(" + _value + ");");
        me.CurHp -= _value;
        this.gameObject.GetComponent<PhotonView>().RPC("PlayerTakeDamage", this.gameObject.GetComponent<PhotonView>().owner, _value);
    }
}
