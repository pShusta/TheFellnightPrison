using UnityEngine;
using System.Collections;

public class DropItem : MonoBehaviour {
    public GameObject InvMain;

    void OnClick()
    {
        foreach(Weapon _w in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().player.InvWeapons){
            if (_w.Id == InvMain.GetComponent<InvUI>().curItem)
            {
                GameObject.FindWithTag("GameController").GetComponent<ControllerV2>().view.RPC("dropWeapon", PhotonTargets.MasterClient, _w.Id, GameObject.FindWithTag("CarryData").GetComponent<CarryData>().player.Username);
            }
        }
    }
}
