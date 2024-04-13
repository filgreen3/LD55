using UnityEngine;

public class OrganTirePanel : MonoBehaviour
{
    [SerializeField] private int _tireLevel;

    public void LoadOrgans(OrganDisplay organDisplayPrefab)
    {
        var organs = Resources.LoadAll<Organ>("Organs/Tier" + _tireLevel);
        foreach (var organ in organs)
        {
            var organDisplay = Instantiate(organDisplayPrefab, transform);
            organDisplay.LoadOrgan(organ);
        }
    }
}

