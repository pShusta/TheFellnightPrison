using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnOnCharacter : MonoBehaviour {

    private bool okay;
    private GameObject Menu, Controller, healthbar;
    public MonoBehaviour script;
    public bool GM;
	// Use this for initialization
	void Start () {
        if (this.gameObject.GetComponent<PhotonView>().isMine)
        {
            this.gameObject.GetComponentInChildren<Camera>().enabled = true;
            this.gameObject.GetComponentInChildren<AudioListener>().enabled = true;
            script.enabled = true;
        }
        else
        {
            this.enabled = false;
        }
	}

    void Update()
    {
    }

    [PunRPC]
    void invitePlayerToParty(PhotonPlayer _inviter)
    {
        GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>().invitePlayerToParty(_inviter);
    }

    [PunRPC]
    void playerJoinParty (PhotonPlayer _player)
    {
        GameObject.FindGameObjectWithTag("MenuController").GetComponentInChildren<SocialScreen>().playerJoinParty(_player);
    }

    [PunRPC]
    void recieveCurrentParty(PhotonPlayer[] _players, bool _open)
    {
        List<PhotonPlayer> _players2 = new List<PhotonPlayer>();
        foreach (PhotonPlayer _p in _players) { _players2.Add(_p); }
        Party _party = new Party(_players2, _open);
        GameObject.FindGameObjectWithTag("MenuController").GetComponentInChildren<SocialScreen>().recieveCurrentParty(_party);
    }
}
