using UnityEngine;

public class TownEnemes : ITownComponent
{
    public Character[] Characters;
    public int EnemyCount;

    public void Generate(TownGenerator generator)
    {
        var population = generator.GetEntityComponent<TownPopulation>();

        for (int i = 0; i < EnemyCount; i++)
        {
            var character = GameObject.Instantiate(Characters[Random.Range(0, Characters.Length)], generator.CenterPosition + Vector3.right * Random.Range(-generator.Size, generator.Size), Quaternion.identity, generator.transform);
            character.GetEntityComponent<Health>().OnDeath += () => population.Population--;
            population.Population++;
        }
    }
}