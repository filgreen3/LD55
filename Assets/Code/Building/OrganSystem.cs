using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganSystem : MonoBehaviour, ISystem
{
    [SerializeField] private TMPro.TMP_Text _limitText;
    [SerializeField] private Organ _baseOrganPrefab;

    public static int OrganLimit => 2 + 2 * WaveSystem.CurrentWave;
    public static int CurrentOrgans => OrganBuilder.ConnectedOrgans.Count;

    public static bool IsFull => CurrentOrgans >= OrganLimit;


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
