using UnityEngine;

public class OrganRender : IOrganComponent
{
    [SerializeField] private SpriteRenderer _renderer;
    public SpriteRenderer Renderer { get => _renderer; set => _renderer = value; }
    public Color Color { get => _renderer.color; set => _renderer.color = value; }
    public Sprite Sprite { get => _renderer.sprite; set => _renderer.sprite = value; }
}