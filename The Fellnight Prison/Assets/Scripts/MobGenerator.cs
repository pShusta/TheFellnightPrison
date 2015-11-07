using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobGenerator : MonoBehaviour {

    public float generationRadius, spawnCooldown;
    private float spawnTimer = 0;
    public int maxSpawned;
    public GameObject mobToSpawn;
    private List<GameObject> spawnedMobs = new List<GameObject>();

	void Start () {
        if (!PhotonNetwork.isMasterClient)
        {
            this.enabled = false;
        }
        else
        {
            
        }
	}

	void Update () {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        if (spawnedMobs.Count < maxSpawned && spawnTimer <= 0)
        {
            GameObject newMob = SpawnMob();
        }
	}

    GameObject SpawnMob()
    {
        float xAxis = Random.Range(-generationRadius, generationRadius);
        float zAxis = Random.Range(-generationRadius, generationRadius);
        GameObject _mobSpawned = PhotonNetwork.Instantiate(mobToSpawn.name, new Vector3(this.transform.position.x + xAxis, this.gameObject.transform.position.y, this.transform.position.z + zAxis), Quaternion.identity, 0);
        spawnedMobs.Add(_mobSpawned);
        _mobSpawned.GetComponent<MobHealth>().setGenerator(this.gameObject);
        spawnTimer = spawnCooldown;
        return _mobSpawned;
    }

    public void RemoveMob(GameObject mobToRemove)
    {
        spawnedMobs.Remove(mobToRemove);
    }
}