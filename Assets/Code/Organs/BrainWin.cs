public class BrainWin : IOrganComponentConnect
{
    public void OnConnect(Organ parent, Organ target)
    {
        WinSystem.Win();
    }

    public void OnDisconnect(Organ parent, Organ target)
    {

    }
}
