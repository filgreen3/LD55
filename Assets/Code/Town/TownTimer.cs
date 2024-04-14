using UnityEngine;
using System.Collections;

public class TownTimer : MonoBehaviour, ISystem
{
    [SerializeField] private TownSystem _townSystem;
    [SerializeField] private TMPro.TMP_Text _timerText;
    [SerializeField] float _timeToBeatTown;

    private float _timer;
    private Coroutine _timerCoroutine;

    private void Start()
    {
        _townSystem.OnTownStart += () => { if (_timerCoroutine != null) StopCoroutine(_timerCoroutine); };
        _townSystem.OnTownStart += () => _timer = _timeToBeatTown;
        _townSystem.OnTownStart += () => _timerCoroutine = StartCoroutine(Timer());
        _townSystem.OnTownStart += () => _timerText.gameObject.SetActive(true);

        _townSystem.OnTownEnd += () => StopCoroutine(_timerCoroutine);
        _townSystem.OnTownEnd += () => _timerText.gameObject.SetActive(false);

    }

    private IEnumerator Timer()
    {
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            _timerText.text = _timer.ToString("TIME:0");
            yield return null;
        }

        BaseOrganComponent.BaseOrgan.GetHealth().Value = 0;
    }
}