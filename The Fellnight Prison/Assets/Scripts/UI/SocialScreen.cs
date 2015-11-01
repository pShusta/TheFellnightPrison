using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SocialScreen : MonoBehaviour {

    private PhotonPlayer[] ConnectedPlayers, nonGmPlayers, GmPlayers;
    public Party CurrentParty;
    private Party[] PublicParties;
    public GameObject[] PlayerButtons, KickButtons, PartyButtons, CurrentParyNames;
    public GameObject MakePublicButton, MakePrivateButton, SendButton, LoadDungeonButton, ChatText, ChatInputField;
    private float? timer = null;

    void Start()
    {
    }

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
	
	void Update () {
        if (timer != null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = null;
                launchDungeon();
            }
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

        if (CurrentParty.Members[0] != PhotonNetwork.player)
        {
            LoadDungeonButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            LoadDungeonButton.GetComponent<Button>().interactable = true;
        }
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
        string roomName = "FellnightPrison" + CurrentParty.Members[0].name;
        PhotonPlayer[] _players = new PhotonPlayer[CurrentParty.Members.Count];
        for (int i = 0; i < CurrentParty.Members.Count; i++)
        {
            _players[i] = CurrentParty.Members[i];
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerV2>().view.RPC("savePlayers", PhotonTargets.MasterClient, _players);

        GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerV2>().view.RPC("gmHostDungeon", GmPlayers[0], roomName, _players);
        wait(3);
    }

    void launchDungeon()
    {
        for (int i = 1; i <= CurrentParty.Members.Count; i++)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerV2>().view.RPC("loadRoom", CurrentParty.Members[CurrentParty.Members.Count - i], "FellnightPrison" + CurrentParty.Members[0].name);
        }
    }

    private void wait(float _time)
    {
        timer = _time;
    }

    public void invitePlayerButton(int _playerNum)
    {
        Debug.Log("invitePlayerButton: " + nonGmPlayers[_playerNum].name);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerV2>().playerToon.GetComponent<PhotonView>().RPC("invitePlayerToParty", nonGmPlayers[_playerNum], PhotonNetwork.player);
    }

    public void kickPlayerButton(int _playerNum)
    {
        PhotonPlayer poorKid = CurrentParty.Members[_playerNum];
        foreach (PhotonPlayer _player in CurrentParty.Members)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerV2>().playerToon.GetComponent<PhotonView>().RPC("kickPlayer", _player, poorKid);
        }
    }

    public void kickPlayer(PhotonPlayer _player)
    {
        if (_player != PhotonNetwork.player)
        {
            CurrentParty.RemoveMember(_player);
            updateCurrentPartyUi();
        }
        else
        {
            CurrentParty = new Party(PhotonNetwork.player);
            updateCurrentPartyUi();
        }
    }

    public void joinPublicPartyButton(int _partyNum)
    {
        Debug.Log("joinPublicPartyButton: " + _partyNum);
    }

    public void joinParty(PhotonPlayer _owner)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerV2>().playerToon.GetComponent<PhotonView>().RPC("playerJoinParty", _owner, PhotonNetwork.player);
    }

    public void playerJoinParty(PhotonPlayer _player)
    {
        int counter = 0;
        CurrentParty.Members.Add(_player);
        List<PhotonPlayer> _players = new List<PhotonPlayer>();
        foreach (PhotonPlayer _p in CurrentParty.Members)
        {
            _players.Add(_p);
            counter++;
        }
        PhotonPlayer[] _players2 = new PhotonPlayer[_players.Count];
        for (int i = 0; i < _players.Count; i++)
        {
            _players2[i] = _players[i];
        }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerV2>().playerToon.GetComponent<PhotonView>().RPC("recieveCurrentParty", _player, _players2, CurrentParty.PartyOpen);
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
