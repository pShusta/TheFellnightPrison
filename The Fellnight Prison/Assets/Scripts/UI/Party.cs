using System.Collections;
using System.Collections.Generic;

public class Party {
    public List<PhotonPlayer> Members;
    public bool PartyOpen;

    public Party(PhotonPlayer _owner)
    {
        Members = new List<PhotonPlayer>();
        Members.Add(_owner);
        PartyOpen = false;
    }

    public Party(List<PhotonPlayer> _players, bool open)
    {
        Members = new List<PhotonPlayer>();
        foreach (PhotonPlayer _player in _players)
        {
            Members.Add(_player);
        }
        PartyOpen = open;
    }

    public void RemoveMember(PhotonPlayer _player)
    {
        Members.Remove(_player);
    }

    public void makePublic()
    {
        PartyOpen = true;
    }

    public void makePrivate()
    {
        PartyOpen = false;
    }
}
