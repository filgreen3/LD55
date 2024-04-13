using UnityEngine;
using System.Collections;

public class TownFakeWin : ITownComponent
{
    public void Generate(TownGenerator generator)
    {
        generator.StartCoroutine(FakeWin(generator));
    }

    private IEnumerator FakeWin(TownGenerator generator)
    {
        yield return new WaitForSeconds(20);
        generator.GetEntityComponent<TownPopulation>().Population = 0;

    }
}
