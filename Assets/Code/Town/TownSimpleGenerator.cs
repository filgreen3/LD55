using UnityEngine;

public class TownSimpleGenerator : ITownComponent
{
    [SerializeField] private Character[] _characters;
    public int SimpleCount;

    public void Generate(TownGenerator generator)
    {
        for (int i = 0; i < SimpleCount; i++)
        {
            var pos = Vector3.right * Random.Range(-generator.Size, generator.Size);
            var character = GameObject.Instantiate(_characters[Random.Range(0, _characters.Length)], generator.CenterPosition + pos, Quaternion.identity, generator.transform);
            if (character.TryGetEntityComponent<CenterTarget>(out var target))
            {
                target.Center = generator.CenterPosition;
            }
        }
    }
}