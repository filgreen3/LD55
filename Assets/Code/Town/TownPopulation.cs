using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TownPopulation : ITownComponent
{
    private int _population;

    public int Population
    {
        get => _population;
        set
        {
            _population = value;
            PopulationChanged?.Invoke(value);
            if (value <= 0) PopulationLost?.Invoke();
        }
    }

    public Action<int> PopulationChanged;
    public Action PopulationLost;

    public void Generate(TownGenerator generator)
    {
        PopulationLost += () => generator.OnTownLost?.Invoke();
    }
}
