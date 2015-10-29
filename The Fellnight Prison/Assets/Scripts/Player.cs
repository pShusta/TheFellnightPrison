using System.Collections;
using System.Collections.Generic;

public class Player {
    public int Str, Agi, Con, Intel, Luck;
    public int OneHandedSword, Gathering, MaxHp;
    public float CurHp;
    public string Username;
    public Weapon Equiped;
    public List<Weapon> InvWeapons;
    public List<CraftingMaterial> InvMaterials;
    
    public Player()
    {
        Str = 10;
        Agi = 10;
        Con = 10;
        Intel = 10;
        Luck = 0;
        MaxHp = Con * 2;
        CurHp = MaxHp;
        InvWeapons = new List<Weapon>();
        InvMaterials = new List<CraftingMaterial>();
    }
    public Player(string _user, int _str, int _agi, int _con, int _intel, int _luck)
    {
        Username = _user;
        Str = _str;
        Agi = _agi;
        Con = _con;
        Intel = _intel;
        Luck = _luck;
        MaxHp = Con * 2;
        CurHp = MaxHp;
        InvWeapons = new List<Weapon>();
        InvMaterials = new List<CraftingMaterial>();
    }
    public Player(string _user, int _str, int _agi, int _con, int _intel, int _luck, int _one, int _gath)
    {
        Username = _user;
        Str = _str;
        Agi = _agi;
        Con = _con;
        Intel = _intel;
        Luck = _luck;
        MaxHp = Con * 2;
        CurHp = MaxHp;
        OneHandedSword = _one;
        Gathering = _gath;
        InvWeapons = new List<Weapon>();
        InvMaterials = new List<CraftingMaterial>();
    }

    public void takeDamage(float _value)
    {
        CurHp -= _value;
    }

    public void EquipWeapon(int _index)
    {
        Equiped = InvWeapons[_index];
    }
}
