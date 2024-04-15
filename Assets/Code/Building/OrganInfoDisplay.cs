using Game.Input;
using UnityEngine;
using UnityEngine.UI;

public class OrganInfoDisplay : MonoBehaviour, ISystem
{
    [SerializeField] private TMPro.TMP_Text _titleText;
    [SerializeField] private TMPro.TMP_Text _mainText;

    private static OrganInfoDisplay _instance;

    private Organ CurrentInfoOrgan
    {
        get => _currentInfoOrgan;
        set
        {
            if (_currentInfoOrgan == value) return;
            if (_currentInfoOrgan != null)
            {
                CurrentInfoOrgan.GetRender().Renderer.SetOutline(false);
            }
            _currentInfoOrgan = value;
            if (value != null)
            {
                DisplayInfo(value);
                CurrentInfoOrgan.GetRender().Renderer.SetOutline(true);
            }
            else
            {
                Clean();
            }
        }
    }
    private Organ _currentInfoOrgan;

    private Collider2D _obj;
    private Organ _organ;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        _obj = Physics2D.OverlapPoint(GetMousePosition(), LayerMask.GetMask("Part"));
        if (_obj && _obj.TryGetComponent<Organ>(out _organ))
        {
            if (_organ != _currentInfoOrgan && _organ.HasEntityComponent(typeof(OrganComponentInfo)))
            {
                CurrentInfoOrgan = _organ;
            }
        }
        else
        {
            CurrentInfoOrgan = null;
        }

    }

    public static void SetInfo(Organ organ) => _instance.DisplayInfo(organ);
    public static void CleanInfo() => _instance.Clean();

    public void DisplayInfo(Organ organ)
    {
        if (organ == null || !organ.TryGetEntityComponent<BasicInfo>(out var organComponentInfo)) return;

        _titleText.text = organComponentInfo.Name;
        _mainText.text = organComponentInfo.Description;
    }

    private void Clean()
    {
        _titleText.text = "";
        _mainText.text = "Some organs need resources<sprite=\"icon\" name=blood>,<sprite=\"icon\" name=energy>. The source of them will be highlighted. Survive all waves. Make <sprite=\"icon\" name=damage> to fight.";
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(GameControl.Instance.Control.MouseLook.ReadValue<Vector2>());
    }
}
