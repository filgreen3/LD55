using UnityEngine;

public class OrganTirePanel : MonoBehaviour
{
    [SerializeField] private int _tireLevel;

    public void LoadOrgans(OrganDisplay organDisplayPrefab)
    {
        var organs = Resources.LoadAll<Organ>("Organs");
        foreach (var organ in organs)
        {
            if (organ == null || organ.GetTier().Tire != _tireLevel) continue;
            var organDisplay = Instantiate(organDisplayPrefab, transform);
            organDisplay.LoadOrgan(organ);
        }
    }
}

