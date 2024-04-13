using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganSystem : MonoBehaviour, ISystem
{
    [SerializeField] private OrganDisplay _organDispayPrefab;
}

public class OrganTirePanel : MonoBehaviour
{
    [SerializeField] private int _tireLevel;

    public void LoadOrgans(OrganDisplay organDisplayPrefab)
    {
        //var organs = Resources.LoadAll<Organ>("Organs/Tier" + _tireLevel);
    }
}

public class OrganDisplay : MonoBehaviour
{
    [SerializeField] private Image _organIcon;
    [SerializeField] private GameObject _organLockIcon;
}

