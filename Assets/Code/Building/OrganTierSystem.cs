using System;
using UnityEngine;

public class OrganTierSystem : MonoBehaviour, ISystem
{
    public static Action OnTierChange;

    private static int _tireLevel = 1;
    public static int TireLevel
    {
        get => _tireLevel;
        private set
        {
            _tireLevel = value;
            OnTierChange?.Invoke();
        }
    }

    [SerializeField] private OrganDisplay _organDispayPrefab;
    [SerializeField] private OrganTirePanel[] _tierPanels;

    private void Start()
    {
        OnTierChange = null;
        TireLevel = 1;
        foreach (var panel in _tierPanels)
        {
            panel.LoadOrgans(_organDispayPrefab);
        }
    }

    public static void UpdateTire()
    {
        var tier = Mathf.RoundToInt((WaveSystem.CurrentWave / (float)TownSystem.AllWaves) * 4);
        TireLevel = Mathf.Clamp(tier, 1, 4);
    }

    private void OnDisable()
    {
        TireLevel = 1;
    }
}


