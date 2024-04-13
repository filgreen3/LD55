using UnityEngine;

public interface IOrganComponent
{

}

public interface IOrganComponentInit : IOrganComponent
{
    public abstract void Init(Organ part);
}

public interface IOrganComponentConnect : IOrganComponent
{
    public abstract void OnConnect(Organ parent, Organ target);
    public abstract void OnDisconnect(Organ parent, Organ target);
}

public interface IOrganComponentUpdate : IOrganComponent
{
    public abstract void Update();
}

public class OrganHealth : IOrganComponent
{
    [SerializeField] private int _value;
    [SerializeField] private int _maxValue;

    public int Value { get => _value; set => _value = value; }
    public int MaxValue { get => _maxValue; set => _maxValue = value; }
    public float Percentage => (float)Value / MaxValue;
    public bool IsDead => Value <= 0;
    public bool IsFull => Value >= MaxValue;
}

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

public class OrganRender : IOrganComponent
{
    [SerializeField] private SpriteRenderer _renderer;
    public SpriteRenderer Renderer { get => _renderer; set => _renderer = value; }
    public Color Color { get => _renderer.color; set => _renderer.color = value; }
    public Sprite Sprite { get => _renderer.sprite; set => _renderer.sprite = value; }
}