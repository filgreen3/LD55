using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStain : MonoBehaviour
{
    [SerializeField] private int _maxBlood;
    [SerializeField] private int _currentBlood;
    [SerializeField] private Transform _transf;

    public int CurrentBlood
    {
        get => _currentBlood; set
        {
            _currentBlood = value;
        }
    }

    private void Start()
    {
        _currentBlood = _maxBlood;
        BloodChange();
    }

    private void BloodChange()
    {
        _transf.localScale = Vector3.one * (_currentBlood / (float)_maxBlood);
        if (_currentBlood <= 0)
        {
            Destroy(gameObject);
        }
    }

}
