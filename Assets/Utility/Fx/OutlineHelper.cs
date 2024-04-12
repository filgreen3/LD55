using UnityEngine;

public class OutlineHelper : MonoBehaviour, ISystem
{
    [SerializeField] private Material _defMaterial;
    [SerializeField] private Material _outlineMaterial;

    private static OutlineHelper _intance;

    public static Material GetMaterialOutline(bool outline) => outline ? _intance._outlineMaterial : _intance._defMaterial;

    private void Awake()
    {
        _intance = this;
    }
}


public static class SpriteRendererOutline
{
    public static void SetOutline(this SpriteRenderer sprite, bool outline)
    {
        sprite.material = OutlineHelper.GetMaterialOutline(outline);
    }
}