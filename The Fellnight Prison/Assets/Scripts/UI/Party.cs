using System.Collections;
using System.Collections.Generic;

public class Party {
    public PhotonPlayer[] Members;
    public bool PartyOpen;

    public Party(PhotonPlayer _owner)
    {
        Members = new PhotonPlayer[6];
        Members[0] = _owner;
        PartyOpen = false;
    }

    public Party(List<PhotonPlayer> _players, bool open)
    {
        Members = new PhotonPlayer[6];
        int counter = 0;
        foreach (PhotonPlayer _player in _players)
        {
            Members[counter] = _player;
            counter++;
        }
        PartyOpen = open;
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
