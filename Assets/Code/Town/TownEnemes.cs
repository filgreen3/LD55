using UnityEngine;
using System.Collections.Generic;

public class TownEnemes : ITownComponent
{
    public const float c_offset = 2f;

    [SerializeField] private Character[] _characters;
    public List<Character> CreatedCharacters = new List<Character>();
    public int EnemyCount;

    public static TownEnemes CurrentTownEnemes;

    public void Generate(TownGenerator generator)
    {
        CurrentTownEnemes = this;
        var population = generator.GetEntityComponent<TownPopulation>();

        for (int i = 0; i < EnemyCount; i++)
        {
            var pos = Vector3.right * (generator.Size + Random.Range(0f, c_offset)) * Mathf.Sign(Random.value - 0.5f);
            var character = GameObject.Instantiate(_characters[Random.Range(0, _characters.Length)], generator.CenterPosition + pos, Quaternion.identity, generator.transform);
            character.GetEntityComponent<Health>().OnDeath += () => population.Population--;

            CreatedCharacters.Add(character);
            character.GetEntityComponent<Health>().OnDeath += () => CreatedCharacters.Remove(character);

            if (character.TryGetEntityComponent<CenterTarget>(out var target))
            {
                target.Center = generator.CenterPosition;
            }
            population.Population++;
        }
    }
}
