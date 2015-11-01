using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{

    public PhotonView myView;
    public GameObject weapon;
    private BoxCollider sword;

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
        Debug.Log("myView.owner.name: " + myView.owner.name);
        Debug.Log("carryData.players.count: " + GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players.Count);
        foreach (Player _p in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players)
        {
            Debug.Log("CarryData.players.Username: " + _p.Username);
            if (_p.Username == myView.owner.name)
            {
                equiped = _p.Equiped;
                Debug.Log("_p.Equiped: " + _p.Equiped + " _p.Equiped.Name: " + _p.Equiped.Name);
                _dmg = equiped.GetPhysDmgAmt();
                Debug.Log("Setting _dmg: " + _dmg);
            }
        }
        
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
}