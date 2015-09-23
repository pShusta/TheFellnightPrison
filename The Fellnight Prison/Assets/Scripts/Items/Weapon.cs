using UnityEngine;
using System;

public class Weapon : BaseItem{

    private int Range, EleDmgAmt, PhysDmgAmt;
    private DmgType PhysDmg;
    private EleDmgType EleDmg;
    private WeaponType WeapType;
    private BaseItem ItemBase;

    public Weapon()
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
        if (Range < 2) {
            WeapType = WeaponType.Low;
        } else if (Range < 4){
            WeapType = WeaponType.Medium;
        } else if (Range < 10) {
            WeapType = WeaponType.High;
        } else if (Range < 30) {
            WeapType = WeaponType.RLow;
        } else {
            WeapType = WeaponType.RHigh;
        }
        ItemBase = new BaseItem();
    }

    public Weapon(DmgType _dmgType, int _physDmgAmt, EleDmgType _eleDmgType, int _eleDmgAmt, int _range)
    {
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
        ItemBase = new BaseItem();
    }

    public Weapon(DmgType _dmgType, int _physDmgAmt, EleDmgType _eleDmgType, int _eleDmgAmt, int _range, int _dura, int _weight)
    {
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
        ItemBase = new BaseItem(_dura, _weight);
    }

	
}
