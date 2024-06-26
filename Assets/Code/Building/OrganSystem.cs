using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganSystem : MonoBehaviour, ISystem
{
    [SerializeField] private TMPro.TMP_Text _limitText;
    [SerializeField] private Organ _baseOrganPrefab;
    [SerializeField] private ShakeText _shakeText;


    private static OrganSystem _instance;
    public static int OrganLimit => Mathf.Clamp(2 + 2 * WaveSystem.CurrentWave, 4, 20);
    public static int CurrentOrgans => OrganBuilder.ConnectedOrgans.Count;
    public static bool IsFull => CurrentOrgans >= OrganLimit;

    public static void TriggerLimit() => _instance._shakeText.Shake();

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        WaveSystem.OnNewWave += (t) => UpdateLimitText();
        OrganBuilder.OnOrganConnectedToMonster += (t) => UpdateLimitText();
        Organ.OrganDestroyedStatic += (t) => UpdateLimitText();
        UpdateLimitText();
        SummonBaseOrgan();
    }

    public void SummonBaseOrgan()
    {
        var organ = Instantiate(_baseOrganPrefab);
        organ.transform.position = Vector3.up * 10f;
    }

    public void UpdateLimitText()
    {
        _limitText.text = $"{CurrentOrgans}/{OrganLimit} Organs";
    }

    public static Organ LoadOrgan(string name)
    {
        var load = Resources.Load<Organ>($"Organs/{name}");
        if (load != null)
            return Instantiate(load);
        return null;
    }
}
