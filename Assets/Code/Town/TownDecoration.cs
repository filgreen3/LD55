using UnityEngine;

public class TownDecoration : ITownComponent
{
    [SerializeField] private GameObject[] _decorations;
    [SerializeField] private int _count;

    public void Generate(TownGenerator generator)
    {
        for (int i = 0; i < _count; i++)
        {
            GameObject.Instantiate(_decorations[Random.Range(0, _decorations.Length)], generator.CenterPosition + Vector3.right * Random.Range(-generator.Size, generator.Size), Quaternion.identity, generator.transform);
        }
    }
}