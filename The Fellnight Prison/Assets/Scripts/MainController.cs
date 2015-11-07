using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainController : MonoBehaviour {
    public GameObject[] Spawns;

	void Start () {
        if (PhotonNetwork.isMasterClient) { 
            LaunchGenerator();
        }
    }

    public void LaunchGenerator()
    {
        Debug.Log("LaunchGenerator()");
        this.gameObject.GetComponent<NewRandRoom>().enabled = true;
    }

    public void FinishGenerator()
    {
        foreach (GameObject generator in GameObject.FindGameObjectsWithTag("MobGenerator"))
        {
            generator.GetComponent<MobGenerator>().enabled = true;
        }
        this.gameObject.GetComponent<PhotonView>().RPC("SpawnPlayer", PhotonTargets.All);
    }

    public void quitButton()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    [PunRPC]
    void SpawnPlayer()
    {
        GameObject _player;
        GameObject _spawn = Spawns[Random.Range(0, Spawns.Length)];
        if (!PhotonNetwork.isMasterClient)
        {
            _player = PhotonNetwork.Instantiate("SkeletonPlayer", _spawn.transform.position, Quaternion.identity, 0);
        }
        else
        {
            _player = PhotonNetwork.Instantiate("GM", _spawn.transform.position, Quaternion.identity, 0);
        }
        if(_player.GetPhotonView().isMine) { _player.SetActive(true); }
    }

    [PunRPC]
    void mobKill(bool itemDrop, int expGain)
    {
        PhotonPlayer[] _players = GameObject.FindWithTag("CarryData").GetComponent<CarryData>().playersView;
        List<Player> _playerList = GameObject.FindWithTag("CarryData").GetComponent<CarryData>().players;

        int partySize = _players.Length;
        int expPerPlayer = expGain / partySize;
        double itemId = 0;
        Weapon _temp = null;
        int playerToRecieveItem = playerToRecieveItem = Random.Range(0, partySize);
        if (itemDrop)
        {
            _temp = new Weapon("Monster Sword");
            itemId = _temp.Id;
        }
        for (int i = 0; i < partySize; i++)
        {
            if (itemDrop && i == playerToRecieveItem)
            {
                this.gameObject.GetComponent<Database>().gainItem(_players[i], itemId);
                GameObject.FindWithTag("GameController").GetComponent<PhotonView>().RPC("gainedItem", _players[i], _temp.Name);
            }
            this.gameObject.GetComponent<Database>().gainExp(_players[i], expPerPlayer, _playerList[i].OneHandedSword);
            int curLevel = (int)Mathf.Floor(Mathf.Log10(_playerList[i].OneHandedSword));
            _playerList[i].OneHandedSword += expPerPlayer;

            GameObject.FindWithTag("GameController").GetComponent<PhotonView>().RPC("gainExp", _players[i], expPerPlayer);

            if ((int)Mathf.Floor(Mathf.Log10(_playerList[i].OneHandedSword)) > curLevel)
            {
                //level-up
            }
        }
    }

    
}
