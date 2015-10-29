using UnityEngine;
using System.Collections;

public class EquipItem : MonoBehaviour {
    public GameObject InvMain;

    void OnClick()
    {
        foreach(Weapon _w in GameObject.FindWithTag("CarryData").GetComponent<CarryData>().player.InvWeapons){
            if (_w.Id == InvMain.GetComponent<InvUI>().curItem){
                GameObject.FindWithTag("GameController").GetComponent<ControllerV2>().view.RPC("equipSwap", PhotonTargets.MasterClient, _w.Id, GameObject.FindWithTag("CarryData").GetComponent<CarryData>().player.Equiped.Id, GameObject.FindWithTag("CarryData").GetComponent<CarryData>().player.Username);
                //GameObject.FindWithTag("GameController").GetComponent<Database>().updateWeaponEquiped(GameObject.FindWithTag("CarryData").GetComponent<CarryData>().player.Equiped.Id, _w.Id);
                GameObject.FindWithTag("CarryData").GetComponent<CarryData>().player.Equiped = _w;
            }
            //database, update new Equiped to 1
        }
        
    }
}
