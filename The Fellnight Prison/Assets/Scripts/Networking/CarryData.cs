using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarryData : MonoBehaviour {

    public Player player;
    public string username, password;
    public string destination;
    public List<Player> players;
    public PhotonPlayer[] playersView;
    public bool ready = false;

	void Start () {
        players = new List<Player>();
        playersView = new PhotonPlayer[0];
        DontDestroyOnLoad(this.gameObject);
	}
}
