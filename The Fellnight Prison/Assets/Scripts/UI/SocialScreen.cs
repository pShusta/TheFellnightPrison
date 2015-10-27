using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SocialScreen : MonoBehaviour {

    private PhotonPlayer[] ConnectedPlayers, nonGmPlayers, GmPlayers;
    private Party CurrentParty;
    private Party[] PublicParties;
    public GameObject[] PlayerButtons, KickButtons, PartyButtons, CurrentParyNames;
    public GameObject MakePublicButton, MakePrivateButton, SendButton, LoadDungeonButton, ChatText, ChatInputField;

    void Start()
    {
        //Debug.Log("PhotonNetwork.player in SocialScreen.Start()" + PhotonNetwork.player);
        //CurrentParty = new Party(PhotonNetwork.player);
    }
    // Use this for initialization
	void OnEnable () {
        if (CurrentParty == null)
        {
            CurrentParty = new Party(PhotonNetwork.player);
        }
        nonGmPlayers = new PhotonPlayer[9];
        GmPlayers = new PhotonPlayer[11];
        ConnectedPlayers = PhotonNetwork.playerList;
        int counter = 0;
        int gmcounter = 0;
        foreach (GameObject _object in PlayerButtons)
        {
            _object.SetActive(false);
        }
        foreach (GameObject _object in PartyButtons)
        {
            _object.SetActive(false);
        }
        foreach (PhotonPlayer _player in ConnectedPlayers)
        {
            //Debug.Log("Name: " + _player.name);
            if (_player.name != "" && _player.name != PhotonNetwork.player.name)
            {
                nonGmPlayers[counter] = _player;
                PlayerButtons[counter].SetActive(true);
                PlayerButtons[counter].GetComponentInChildren<Text>().text = _player.name;
                counter++;
            }
            else if (_player.name != PhotonNetwork.player.name && !_player.isMasterClient)
            {
                GmPlayers[gmcounter] = _player;
                gmcounter++;
            }
        }
        updateCurrentPartyUi();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void makePublicButton()
    {
        Debug.Log("makePublicButton");
    }

    public void makePrivateButton()
    {
        Debug.Log("makePrivateButton");
    }

    public void sendButton()
    {
        Debug.Log("sendButton");
    }

    public void loadDungeonButton()
    {
        Debug.Log("loadDungeonButton");
    }

    public void invitePlayerButton(int _playerNum)
    {
        Debug.Log("invitePlayerButton: " + nonGmPlayers[_playerNum].name);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>().PlayerToon.GetComponent<PhotonView>().RPC("invitePlayerToParty", nonGmPlayers[_playerNum], PhotonNetwork.player);
    }

    public void kickPlayerButton(int _playerNum)
    {
        Debug.Log("kickPlayerButton: " + _playerNum);
    }

    public void joinPublicPartyButton(int _partyNum)
    {
        Debug.Log("joinPublicPartyButton: " + _partyNum);
    }

    public void joinParty(PhotonPlayer _owner)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>().PlayerToon.GetComponent<PhotonView>().RPC("playerJoinParty", _owner, PhotonNetwork.player);
    }

    public void playerJoinParty(PhotonPlayer _player)
    {
        int counter = 0;
        List<PhotonPlayer> _players = new List<PhotonPlayer>();
        foreach (PhotonPlayer _p in CurrentParty.Members)
        {
            _players.Add(_p);
            counter++;
        }
        CurrentParty.Members[counter] = _player;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>().PlayerToon.GetComponent<PhotonView>().RPC("playerJoinParty", _player, _players, CurrentParty.PartyOpen);
        updateCurrentPartyUi();
    }

    void updateCurrentPartyUi()
    {
        int counter = 0;
        foreach (GameObject _object in KickButtons)
        {
            _object.SetActive(false);
        }
        foreach (GameObject _object in CurrentParyNames)
        {
            _object.SetActive(false);
        }
        foreach (PhotonPlayer _player in CurrentParty.Members)
        {
            CurrentParyNames[counter].SetActive(true);
            if (_player.name != "")
            {
                CurrentParyNames[counter].GetComponent<Text>().text = _player.name;
            }
            else
            {
                CurrentParyNames[counter].GetComponent<Text>().text = "Master";
            }
            KickButtons[counter].SetActive(true);
            counter++;
        }
    }

    public void recieveCurrentParty(Party _party)
    {
        CurrentParty = _party;
        updateCurrentPartyUi();
    }
}
