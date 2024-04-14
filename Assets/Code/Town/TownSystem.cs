using UnityEngine;
using UnityEngine.UI;

public class TownSystem : MonoBehaviour, ISystem
{
    [SerializeField] private Button _buttonToStart;
    [SerializeField] private Transform _cameraTartget;
    [SerializeField] private Organ _monsterBaseOrgan;
    [SerializeField] private Transform _monsterBaseTransform;
    [SerializeField] private TownGenerator[] _townGenerators;

    private TownGenerator _currentTownGenerator;

    private void Start()
    {
        _buttonToStart.onClick.AddListener(GenerateRandomTown);
    }

    private void GenerateRandomTown()
    {
        GenerateTown(_townGenerators[Random.Range(0, _townGenerators.Length)]);
    }

    public void GenerateTown(TownGenerator generator)
    {
        if (_currentTownGenerator != null)
            Destroy(_currentTownGenerator.gameObject);

        var pos = Vector3.right * Random.Range(10, 15);
        _currentTownGenerator = Instantiate(generator, pos, Quaternion.identity, transform);
        _currentTownGenerator.Generate(_currentTownGenerator);
        _cameraTartget.position = _currentTownGenerator.transform.position.x * Vector3.right;
        _currentTownGenerator.OnTownLost += () => _cameraTartget.position = Vector3.zero;
        _currentTownGenerator.OnTownLost += () => Clean();
        _currentTownGenerator.OnTownLost += () => WaveSystem.CurrentWave++;

        MoveMonster(_currentTownGenerator.transform.position);
    }

    public void Clean()
    {
        if (_currentTownGenerator != null)
            Destroy(_currentTownGenerator.gameObject);
    }

    public void MoveMonster(Vector3 pos)
    {
        _monsterBaseOrgan.Rig.bodyType = RigidbodyType2D.Dynamic;
        _monsterBaseTransform.position = pos + Vector3.up * 10;
    }
}
