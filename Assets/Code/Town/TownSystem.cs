using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class TownSystem : MonoBehaviour, ISystem
{
    [SerializeField] private Button _buttonToStart;
    [SerializeField] private Transform _cameraTartget;
    [SerializeField] private OrganBuilder _organBuilder;
    [SerializeField] private TownGenerator _kingTown;
    [SerializeField] private TownGenerator[] _townGenerators;


    public Action OnTownStart;
    public Action OnTownEnd;

    private TownGenerator _currentTownGenerator;
    private static TownSystem _instance;

    public static bool IsBattle;
    public static int AllWaves => _instance._townGenerators.Length;

    private void Awake()
    {
        IsBattle = false;
        _instance = this;
    }

    private void Start()
    {
        _buttonToStart.onClick.AddListener(GenerateRandomTown);
    }

    private void GenerateRandomTown()
    {
        _organBuilder.CanBuild = false;
        AudioHelper.PlayClip(ClipStorage.Instance._summon, 0.1f);
        if (WinSystem.ReadyToWin)
        {
            GenerateTown(_kingTown);
        }
        else
        {
            GenerateTown(_townGenerators[Mathf.Clamp(WaveSystem.CurrentWave, 0, AllWaves - 1)]);
        }
        OnTownStart?.Invoke();
        ResourceParticleHelperSystem.Hide();
        _buttonToStart.gameObject.SetActive(false);
        HideOpenBuilder.Hide();
    }

    public void GenerateTown(TownGenerator generator)
    {
        if (_currentTownGenerator != null)
            Destroy(_currentTownGenerator.gameObject);

        var pos = Vector3.right * 15f;
        _currentTownGenerator = Instantiate(generator, pos, Quaternion.identity, transform);
        _currentTownGenerator.Generate(_currentTownGenerator);
        _cameraTartget.position = _currentTownGenerator.transform.position.x * Vector3.right;
        _currentTownGenerator.OnTownLost += TownLost;
        MoveMonster(_currentTownGenerator.transform.position);
        IsBattle = true;
        LineHelperSystem.SetAlpha(0);
    }

    public void Clean()
    {
        if (_currentTownGenerator != null)
            Destroy(_currentTownGenerator.gameObject);
    }

    public void TownLost()
    {
        AudioHelper.PlayClip(ClipStorage.Instance._win, 0.1f);
        StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        OnTownEnd?.Invoke();
        WaveSystem.CurrentWave++;
        OrganTierSystem.UpdateTire();
        yield return new WaitForSeconds(1f);
        Back();
    }

    public void Back()
    {
        OnTownEnd?.Invoke();
        IsBattle = false;
        _cameraTartget.position = Vector3.right * 2;
        MoveMonster(Vector2.zero);
        Clean();
        _organBuilder.Heal();
        _organBuilder.CanBuild = true;
        _buttonToStart.gameObject.SetActive(true);
        HideOpenBuilder.Show();
        LineHelperSystem.SetAlpha(1);
        ResourceParticleHelperSystem.Hide();
    }

    public static void TownReset() => _instance.Back();

    public void MoveMonster(Vector3 pos)
    {
        pos += Vector3.up * 10;
        BaseOrganComponent.BaseOrgan.Rig.bodyType = RigidbodyType2D.Dynamic;

        foreach (var organ in OrganBuilder.ConnectedOrgans)
        {
            organ.Rig.bodyType = RigidbodyType2D.Static;
        }

        for (int i = OrganBuilder.ConnectedOrgans.Count - 1; i >= 0; i--)
        {
            OrganBuilder.ConnectedOrgans[i].Rig.position = (Vector2)pos + (OrganBuilder.ConnectedOrgans[i].Rig.position - BaseOrganComponent.BaseOrgan.Rig.position);
        }

        foreach (var organ in OrganBuilder.ConnectedOrgans)
        {
            organ.Rig.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnDisable()
    {
        OnTownStart = null;
        OnTownEnd = null;
        IsBattle = false;
    }
}
