using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{

    public GameObject weapon;
    private BoxCollider sword;

    // Use this for initialization
    void Start()
    {
        sword = weapon.GetComponent<BoxCollider>();


    }
    void OnCollisionEnter(Collision sword)
    {
        if (sword.gameObject.name == "target")
        {

            Debug.Log("you hit a target");
            Destroy(sword.gameObject);


        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}