using UnityEngine;

public class OrganTestComp : IOrganComponentConnect, IOrganComponentInit, IOrganComponentUpdate
{
    public void Init(Organ part)
    {
        Debug.Log("Init");
    }

    public void OnConnect(Organ parent, Organ target)
    {
        Debug.Log("Connect");
    }

    public void OnDisconnect(Organ parent, Organ target)
    {
        Debug.Log("Disconnect");
    }

    public void Update()
    {
        Debug.Log("Update");
    }
}
