public class FriendClass {

    private string f1, f2, f3, f4, f5, name;

    public FriendClass(string _f1, string _f2, string _f3, string _f4, string _f5, string _name)
    {
        f1 = _f1; f2 = _f2; f3 = _f3; f4 = _f4; f5 = _f5; name = _name;
    }

    public string getf1() { return f1; }
    public string getf2() { return f2; }
    public string getf3() { return f3; }
    public string getf4() { return f4; }
    public string getf5() { return f5; }
    void setf1(string f) { f1 = f; }
    void setf2(string f) { f2 = f; }
    void setf3(string f) { f3 = f; }
    void setf4(string f) { f4 = f; }
    void setf5(string f) { f5 = f; }
    public string getname() { return name; }
    void setname(string n) { name = n; }

}
