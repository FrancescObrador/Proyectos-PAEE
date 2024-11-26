
public class deleteTaskEventArgs
{
    public int list { get; set; }
    public int task { get; set; }
    public deleteTaskEventArgs(int l, int t) : base()
    {
        list = l;
        task = t;
    }
}