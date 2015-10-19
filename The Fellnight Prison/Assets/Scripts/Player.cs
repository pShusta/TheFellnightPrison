using System.Collections;

public class Player {
    public int Str, Agi, Con, Intel, Luck;
    public string Username;

    public Player()
    {
        Str = 10;
        Agi = 10;
        Con = 10;
        Intel = 10;
        Luck = 0;
    }
    public Player(string _user, int _str, int _agi, int _con, int _intel, int _luck)
    {
        Username = _user;
        Str = _str;
        Agi = _agi;
        Con = _con;
        Intel = _intel;
        Luck = _luck;
    }
}
