using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OrganDisplay : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _organIcon;
    [SerializeField] private GameObject _organLockIcon;

    public Organ CurrentOrgan { get; set; }

    public void LoadOrgan(Organ organ)
    {
        CurrentOrgan = organ;
        _organIcon.sprite = organ.GetRender().Sprite;
        _organLockIcon.SetActive(!organ.GetTier().IsOpen);
        OrganTierSystem.OnTierChange += () => _organLockIcon.SetActive(!organ.GetTier().IsOpen);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CurrentOrgan == null || OrganSystem.IsFull || !CurrentOrgan.GetTier().IsOpen) return;
        OrganBuilder.CallToBuild(Instantiate(CurrentOrgan, Vector3.down * 1000f, Quaternion.identity));
    }
}

