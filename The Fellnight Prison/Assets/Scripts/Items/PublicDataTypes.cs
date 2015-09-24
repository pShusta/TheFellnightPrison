

public class PublicDataTypes {
    public enum DmgType { Blugeoning, Piercing, Slashing };
    public enum EleDmgType { Electric, Fire, Water, Rock, Acid, None };
    public enum WeaponType { RLow, RHigh, Low, Medium, High };
    public enum ArmorType { Light, Medium, Heavy };
    public enum OtherType { Consumable };
    public enum QuestItem { Daily, Story, Side };

    public static DmgType ToDmgType(string _value)
    {
        if (_value == DmgType.Blugeoning.ToString())
        {
            return DmgType.Blugeoning;
        }
        else if (_value == DmgType.Piercing.ToString())
        {
            return DmgType.Piercing;
        }
        else
        {
            return DmgType.Slashing;
        }
    }
    public static EleDmgType ToEleDmgType(string _value)
    {
        if (_value == EleDmgType.Acid.ToString())
        {
            return EleDmgType.Acid;
        }
        else if (_value == EleDmgType.Electric.ToString())
        {
            return EleDmgType.Electric;
        }
        else if (_value == EleDmgType.Fire.ToString())
        {
            return EleDmgType.Fire;
        }
        else if (_value == EleDmgType.Water.ToString())
        {
            return EleDmgType.Water;
        }
        else if (_value == EleDmgType.Rock.ToString())
        {
            return EleDmgType.Rock;
        }
        else
        {
            return EleDmgType.None;
        }
    }
}
