using UnityEngine;
using System.Collections;

public class BaseItem : PublicDataTypes {

    private int Durability, Weight;

    public BaseItem()
    {
        Durability = Random.Range(0, 100);
        Weight = Random.Range(0, 100);
    }

    public BaseItem(int _dura, int _weight)
    {
        Durability = _dura;
        Weight = _weight;
    }

    public void SetDura(int _value) { Durability = _value; }
    public int GetDura() { return Durability; }
    public void SetWeight(int _value) { Weight = _value; }
    public int GetWeight() { return Weight; }
}
