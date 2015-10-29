using UnityEngine;
using System.Collections;

public class BaseItem : PublicDataTypes {

    public double Id;
    public string Name;
    private int Durability, Weight;

    public BaseItem()
    {
        Name = "Default";
        Durability = Random.Range(0, 100);
        Weight = Random.Range(0, 100);
    }

    public BaseItem(string _name)
    {
        Name = _name;
        Durability = Random.Range(0, 100);
        Weight = Random.Range(0, 100);
    }

    public BaseItem(string _name, int _dura, int _weight)
    {
        Name = _name;
        Durability = _dura;
        Weight = _weight;
    }

    public void SetDura(int _value) { Durability = _value; }
    public int GetDura() { return Durability; }
    public void SetWeight(int _value) { Weight = _value; }
    public int GetWeight() { return Weight; }
}
