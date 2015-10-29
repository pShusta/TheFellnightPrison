using UnityEngine;
using System;

public class CraftingMaterial : BaseItem
{

    private string Name;
    private double Id;

    public CraftingMaterial(string _name = "Name: Default")
        : base()
    {
        Name = _name;
        Id = Database.MaterialDbInsert(this);
    }

    public CraftingMaterial(string _name, int _dura, int _weight)
        : base(_name, _dura, _weight)
    {
        Id = Database.MaterialDbInsert(this);
    }

    public CraftingMaterial(int id, string _name, int _dura, int _weight)
        : base(_name, _dura, _weight)
    {
        Id = id;
    }

    public void SetName(string _value) { Name = _value; }
    public string GetName() { return Name; }
    public double GetId() { return Id; }

}
