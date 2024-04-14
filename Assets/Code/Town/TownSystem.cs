using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class TownSystem : MonoBehaviour, ISystem
{
    [SerializeField] private Button _buttonToStart;
    [SerializeField] private Transform _cameraTartget;
    [SerializeField] private OrganBuilder _organBuilder;
    [SerializeField] private TownGenerator[] _townGenerators;

    public Action OnTownStart;
    public Action OnTownEnd;

    private TownGenerator _currentTownGenerator;

    private void Start()
    {
        _buttonToStart.onClick.AddListener(GenerateRandomTown);
    }

    private void GenerateRandomTown()
    {
        _organBuilder.CanBuild = false;
        GenerateTown(_townGenerators[UnityEngine.Random.Range(0, _townGenerators.Length)]);
        OnTownStart?.Invoke();
    }

    public void GenerateTown(TownGenerator generator)
    {
        if (_currentTownGenerator != null)
            Destroy(_currentTownGenerator.gameObject);

        var pos = Vector3.right * UnityEngine.Random.Range(10, 15);
        _currentTownGenerator = Instantiate(generator, pos, Quaternion.identity, transform);
        _currentTownGenerator.Generate(_currentTownGenerator);
        _cameraTartget.position = _currentTownGenerator.transform.position.x * Vector3.right;
        _currentTownGenerator.OnTownLost += TownLost;
        MoveMonster(_currentTownGenerator.transform.position);
    }

    public void Clean()
    {
        if (_currentTownGenerator != null)
            Destroy(_currentTownGenerator.gameObject);
    }

    public void TownLost()
    {
        _cameraTartget.position = Vector3.zero;
        MoveMonster(Vector3.zero);
        Clean();
        WaveSystem.CurrentWave++;
        _organBuilder.CanBuild = true;
        OnTownEnd?.Invoke();
        OrganTierSystem.TireLevel++;
    }
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
}
