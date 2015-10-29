using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarryData : MonoBehaviour {

    public Player player;
    public string username, password;
    public string destination;
    public List<Player> players = new List<Player>();
    public bool ready = false;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
	}

}
