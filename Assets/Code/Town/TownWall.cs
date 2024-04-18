using UnityEngine;

public class TownWall : ITownComponent
{
    [SerializeField] private GameObject _wall;

    public void Generate(TownGenerator generator)
    {
        GameObject.Instantiate(_wall, generator.CenterPosition + Vector3.right * (generator.Size + TownEnemes.c_offset + 0.25f), Quaternion.identity, generator.transform);
        GameObject.Instantiate(_wall, generator.CenterPosition + Vector3.left * (generator.Size + TownEnemes.c_offset + 0.25f), Quaternion.identity, generator.transform);
    }
}