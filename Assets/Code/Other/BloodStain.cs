using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStain : MonoBehaviour
{
    [SerializeField] private int _maxBlood;
    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _bloods;

    private Dictionary<Transform, float> _bloodsSizes;
    private int _currentBlood;

    public int CurrentBlood
    {
        get => _currentBlood; set
        {
            _currentBlood = value;
        }
    }

    private IEnumerator Start()
    {
        _currentBlood = _maxBlood;
        _bloodsSizes = new Dictionary<Transform, float>();
        for (int i = 0; i < _bloods.Length; i++)
        {
            _bloodsSizes.Add(_bloods[i], Random.Range(0f, 1f));
        }


        foreach (var item in _bloods)
        {
            item.gameObject.SetActive(true);
            BloodChange(item, 0);
            yield return null;
        }

        var t = 0f;
        while (t < 1)
        {
            foreach (var item in _bloods)
            {
                t += Time.deltaTime * _speed;
                BloodChange(item, t);
                yield return null;
            }
        }
    }

    private void BloodChange(Transform transf, float value)
    {
        transf.localScale = Vector3.one * (_currentBlood / (float)_maxBlood) * _bloodsSizes[transf] * value;
    }
}
