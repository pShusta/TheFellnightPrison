using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{

    public PhotonView myView;
    public GameObject weapon;
    private BoxCollider sword;

    // Use this for initialization
    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            sword = weapon.GetComponent<BoxCollider>();
        }
        else
        {
            this.enabled = false;
        }
        


    }
    void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        Weapon equiped;
        int _dmg = 0;
        Debug.Log("You hit: " + other.gameObject.name);
        foreach (Player _p in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
        {
            if (_p.Username == myView.owner.name)
            {
                equiped = _p.Equiped;
                _dmg = equiped.GetPhysDmgAmt();
                Debug.Log("Setting _dmg: " + _dmg);
                //temporary for alpha, set to flat 5
                //_dmg = 5;
                //get weapon
            }
        }
                
        //other.gameObject.GetComponent<HitBox>().takeDamage(_dmg);
        MonoBehaviour _script = other.gameObject.GetComponent<HitBox>();
        if (_script != null)
        {
            other.gameObject.GetComponent<HitBox>().takeDamage(_dmg);
        }
        else
        {
            Debug.Log("Immortal Object");
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}