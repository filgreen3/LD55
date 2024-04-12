using UnityEngine;

namespace Game.Parts
{
    public interface IPartComponent
    {

    }

    public interface IPartComponentInit : IPartComponent
    {
        public abstract void Init(Part part);
    }

    public interface IPartComponentConnect : IPartComponent
    {
        public abstract void OnConnect(Part parent, Part target);
        public abstract void OnDisconnect(Part parent, Part target);
    }

    public interface IPartComponentUpdate : IPartComponent
    {
        public abstract void Update();
    }

    public class PartHealth : IPartComponent
    {
        [SerializeField] private int _value;
        [SerializeField] private int _maxValue;

        public int Value { get => _value; set => _value = value; }
        public int MaxValue { get => _maxValue; set => _maxValue = value; }
        public float Percentage => (float)Value / MaxValue;
        public bool IsDead => Value <= 0;
        public bool IsFull => Value >= MaxValue;
    }

    public class PartTestComp : IPartComponentConnect, IPartComponentInit, IPartComponentUpdate
    {
        public void Init(Part part)
        {
            Debug.Log("Init");
        }

        public void OnConnect(Part parent, Part target)
        {
            Debug.Log("Connect");
        }

        public void OnDisconnect(Part parent, Part target)
        {
            Debug.Log("Disconnect");
        }

        public void Update()
        {
            Debug.Log("Update");
        }
    }

    public class PartRender : IPartComponent
    {
        [SerializeField] private SpriteRenderer _renderer;
        public SpriteRenderer Renderer { get => _renderer; set => _renderer = value; }
        public Color Color { get => _renderer.color; set => _renderer.color = value; }
        public Sprite Sprite { get => _renderer.sprite; set => _renderer.sprite = value; }
    }
}