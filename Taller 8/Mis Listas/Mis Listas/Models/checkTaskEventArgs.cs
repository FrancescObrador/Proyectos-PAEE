
public class checkTaskEventArgs
{
    public int list { get; set; }
    public int task { get; set; }
    public checkTaskEventArgs(int l, int t) : base()
    {
        list = l;
        task = t;
    }
}