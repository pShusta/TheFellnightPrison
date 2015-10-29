using UnityEngine;
using System;

public class Weapon : BaseItem{

    
    private int Range, EleDmgAmt, PhysDmgAmt;
    private DmgType PhysDmg;
    private EleDmgType EleDmg;
    private WeaponType WeapType;
    private BaseItem ItemBase;

    public Weapon(string _name = "Random Weapon") : base(_name)
    {
        if (_name != "Monster Sword")
        {
            Range = UnityEngine.Random.Range(1, 20);
            Array values = EleDmgType.GetValues(typeof(EleDmgType));
            System.Random random = new System.Random();
            EleDmg = (EleDmgType)values.GetValue(random.Next(values.Length));
            values = DmgType.GetValues(typeof(DmgType));
            random = new System.Random();
            PhysDmg = (DmgType)values.GetValue(random.Next(values.Length));
            if (EleDmg != EleDmgType.None)
            {
                EleDmgAmt = UnityEngine.Random.Range(0, 100);
            }
            else
            {
                EleDmgAmt = 0;
            }
            PhysDmgAmt = UnityEngine.Random.Range(0, 60);
            if (Range < 2)
            {
                WeapType = WeaponType.Low;
            }
            else if (Range < 4)
            {
                WeapType = WeaponType.Medium;
            }
            else if (Range < 10)
            {
                WeapType = WeaponType.High;
            }
            else if (Range < 30)
            {
                WeapType = WeaponType.RLow;
            }
            else
            {
                WeapType = WeaponType.RHigh;
            }
            Id = Database.WeaponDbInsert(this);
        }
        else
        {
            Name = _name;
            Range = 1;
            EleDmg = EleDmgType.None;
            PhysDmg = DmgType.Slashing;
            EleDmgAmt = 0;
            PhysDmgAmt = UnityEngine.Random.Range(10, 26);
            WeapType = WeaponType.Low;
            Id = Database.WeaponDbInsert(this);
        }
    }

    public Weapon(string _name, DmgType _dmgType, int _physDmgAmt, EleDmgType _eleDmgType, int _eleDmgAmt, int _range) : base()
    {
        Name = _name;
        Id = (double)UnityEngine.Random.Range(0, 1000000000);
        Range = _range;
        EleDmgAmt = _eleDmgAmt;
        PhysDmgAmt = _physDmgAmt;
        PhysDmg = _dmgType;
        EleDmg = _eleDmgType;
        if (Range < 2)
        {
            WeapType = WeaponType.Low;
        }
        else if (Range < 4)
        {
            WeapType = WeaponType.Medium;
        }
        else if (Range < 10)
        {
            WeapType = WeaponType.High;
        }
        else if (Range < 30)
        {
            WeapType = WeaponType.RLow;
        }
        else
        {
            WeapType = WeaponType.RHigh;
        }
        Id = Database.WeaponDbInsert(this);
    }

    public Weapon(string _name, DmgType _dmgType, int _physDmgAmt, EleDmgType _eleDmgType, int _eleDmgAmt, int _range, int _dura, int _weight) : base(_name, _dura, _weight)
    {
        Id = (double)UnityEngine.Random.Range(0, 1000000000);
        Range = _range;
        EleDmgAmt = _eleDmgAmt;
        PhysDmgAmt = _physDmgAmt;
        PhysDmg = _dmgType;
        EleDmg = _eleDmgType;
        if (Range < 2)
        {
            WeapType = WeaponType.Low;
        }
        else if (Range < 4)
        {
            WeapType = WeaponType.Medium;
        }
        else if (Range < 10)
        {
            WeapType = WeaponType.High;
        }
        else if (Range < 30)
        {
            WeapType = WeaponType.RLow;
        }
        else
        {
            WeapType = WeaponType.RHigh;
        }
        Id = Database.WeaponDbInsert(this);
    }

    public Weapon(int id, string _name, DmgType _dmgType, int _physDmgAmt, EleDmgType _eleDmgType, int _eleDmgAmt, int _range, int _dura, int _weight)
        : base(_name, _dura, _weight)
    {
        Id = id;
        Range = _range;
        EleDmgAmt = _eleDmgAmt;
        PhysDmgAmt = _physDmgAmt;
        PhysDmg = _dmgType;
        EleDmg = _eleDmgType;
        if (Range < 2)
        {
            WeapType = WeaponType.Low;
        }
        else if (Range < 4)
        {
            WeapType = WeaponType.Medium;
        }
        else if (Range < 10)
        {
            WeapType = WeaponType.High;
        }
        else if (Range < 30)
        {
            WeapType = WeaponType.RLow;
        }
        else
        {
            WeapType = WeaponType.RHigh;
        }
        //Id = Database.WeaponDbInsert(this);
    }
    
    public void SetName(string _value) { Name = _value; }
    public string GetName() { return Name; }
    public double GetId() { return Id; }
    public void SetRange(int _value) { Range = _value; }
    public int GetRange() { return Range; }
    public void SetDmgType(DmgType _value) { PhysDmg = _value; }
    public DmgType GetDmgType() { return PhysDmg; }
    public void SetPhysDmgAmt(int _value) { PhysDmgAmt = _value; }
    public int GetPhysDmgAmt() { return PhysDmgAmt; }
    public void SetEleDmgType(EleDmgType _value) { EleDmg = _value; }
    public EleDmgType GetEleDmgType() { return EleDmg; }
    public void SetEleDmgAmt(int _value) { EleDmgAmt = _value; }
    public int GetEleDmgAmt() { return EleDmgAmt; }
    public void SetWeapType(WeaponType _value) { WeapType = _value; }
    public WeaponType GetWeapType() { return WeapType; }
	
}
