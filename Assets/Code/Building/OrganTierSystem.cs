using System;
using UnityEngine;

public class OrganTierSystem : MonoBehaviour, ISystem
{

    public static Action OnTierChange;

    private static int _tireLevel = 1;
    public static int TireLevel
    {
        get => _tireLevel; private set
        {
            _tireLevel = value;
            OnTierChange?.Invoke();
        }
    }

    [SerializeField] private OrganDisplay _organDispayPrefab;
    [SerializeField] private OrganTirePanel[] _tierPanels;

    private void Start()
    {
        foreach (var panel in _tierPanels)
        {
            panel.LoadOrgans(_organDispayPrefab);
        }
    }


}

