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
        Check();
        OrganTierSystem.OnTierChange += Check;
    }

    private void OnDisable()
    {
        OrganTierSystem.OnTierChange -= Check;
    }

    private void Check()
    {
        _organLockIcon.SetActive(!CurrentOrgan.GetTier().IsOpen);
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
        if (CurrentOrgan == null) return;
        OrganInfoDisplay.SetInfo(CurrentOrgan);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CurrentOrgan == null) return;
        OrganInfoDisplay.CleanInfo();
    }
}

