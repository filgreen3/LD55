using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game.Input;
using UnityEngine.SceneManagement;

public class LoseSystem : MonoBehaviour, ISystem
{
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private CanvasGroup _loseScreenGroup;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TMPro.TMP_Text _loseText;

    private void Start()
    {
        Time.timeScale = 1;
        BaseOrganComponent.AddAction(Subscribe);
        GameControl.Instance.Control.Rotate.performed += ctx => Restart();
    }

    private void Subscribe(Organ organ)
    {
        organ.GetHealth().OnDeath += Show;
        organ.OnOrganDestroyed += (t) => Show();
    }

    public void Show()
    {
        if (this == null) return;
        AudioHelper.PlayClip(ClipStorage.Instance._lose, 0.1f);
        _loseScreenGroup.alpha = 0;
        _loseScreenGroup.blocksRaycasts = true;
        _loseScreenGroup.interactable = true;
        _restartButton.gameObject.SetActive(false);
        _loseText.gameObject.SetActive(false);
        _restartButton.onClick.AddListener(Restart);
        _loseScreen.SetActive(true);
        StartCoroutine(Animation());
    }

    public void Restart()
    {
        GameControl.Instance.Control.Rotate.performed -= ctx => Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator Animation()
    {
        while (_loseScreenGroup.alpha < 1)
        {
            _loseScreenGroup.alpha += Time.fixedDeltaTime * 2;
            Time.timeScale = 1 - _loseScreenGroup.alpha * 0.9f;
            yield return new WaitForFixedUpdate();
        }
        yield return TextAnimation();
    }

    private IEnumerator TextAnimation()
    {
        var text = _loseText.text;
        _loseText.text = "";
        _loseText.gameObject.SetActive(true);
        for (int i = 0; i < text.Length; i++)
        {
            _loseText.text = text.Substring(0, i + 1);
            yield return new WaitForFixedUpdate();
        }
        _restartButton.gameObject.SetActive(true);
    }
}
