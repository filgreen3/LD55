using Game.Input;
using UnityEngine;
using UnityEngine.UI;

public class OrganInfoDisplay : MonoBehaviour, ISystem
{
    [SerializeField] private TMPro.TMP_Text _titleText;
    [SerializeField] private TMPro.TMP_Text _mainText;

    private Organ CurrentInfoOrgan
    {
        get => _currentInfoOrgan;
        set
        {
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

    private void DisplayInfo(Organ organ)
    {
        var organComponentInfo = organ.GetEntityComponent<OrganComponentInfo>();
        _titleText.text = organComponentInfo.Name;
        _mainText.text = organComponentInfo.Description;
    }

    private void Clean()
    {
        _titleText.text = "";
        _mainText.text = "";
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(GameControl.Instance.Control.MouseLook.ReadValue<Vector2>());
    }
}
