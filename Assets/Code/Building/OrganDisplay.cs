using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OrganDisplay : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
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
        if (CurrentOrgan == null || !CurrentOrgan.GetTier().IsOpen) return;
        if (OrganSystem.IsFull)
        {
            OrganSystem.TriggerLimit();
            return;
        }
        OrganBuilder.CallToBuild(Instantiate(CurrentOrgan, Vector3.down * 1000f, Quaternion.identity));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CurrentOrgan == null || !CurrentOrgan.GetTier().IsOpen) return;
        OrganInfoDisplay.SetInfo(CurrentOrgan);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CurrentOrgan == null || !CurrentOrgan.GetTier().IsOpen) return;
        OrganInfoDisplay.CleanInfo();
    }
}

