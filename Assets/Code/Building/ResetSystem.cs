using UnityEngine;
using Game.Input;

public class ResetSystem : MonoBehaviour, ISystem
{
    [SerializeField] private GameObject _resetText;
    [SerializeField] private TownSystem _townSystem;

    private void Start()
    {
        GameControl.Instance.Control.Rotate.performed += ctx => Restart();
        _townSystem.OnTownStart += () => _resetText.SetActive(true);
        _townSystem.OnTownEnd += () => _resetText.SetActive(false);
    }

    public void Restart()
    {
        if (!TownSystem.IsBattle) return;
        GameControl.Instance.Control.Rotate.performed -= ctx => Restart();
        TownSystem.TownReset();
    }
}