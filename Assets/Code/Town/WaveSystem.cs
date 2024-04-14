using System;
using UnityEngine;

public class WaveSystem : MonoBehaviour, ISystem
{
    [SerializeField] private TMPro.TMP_Text _waveText;

    private static WaveSystem _instance;
    private static int _currentWave = 1;

    public static Action<int> OnNewWave;

    public static int CurrentWave
    {
        get => _currentWave; set
        {
            _currentWave = value;
            _instance.UpdateText();
            OnNewWave?.Invoke(_currentWave);
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _waveText.text = $"{CurrentWave}/{TownSystem.AllWaves} Wave";
    }
}
