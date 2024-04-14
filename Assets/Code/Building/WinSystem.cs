using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinSystem : MonoBehaviour, ISystem
{
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private CanvasGroup _winScreenGroup;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TMPro.TMP_Text _winText;

    private static WinSystem _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static void Win()
    {
        _instance.Show();
    }

    public void Show()
    {
        if (this == null) return;
        _winScreenGroup.alpha = 0;
        _winScreenGroup.blocksRaycasts = true;
        _winScreenGroup.interactable = true;
        _restartButton.gameObject.SetActive(false);
        _winText.gameObject.SetActive(false);
        _restartButton.onClick.AddListener(Restart);
        _winScreen.SetActive(true);
        StartCoroutine(Animation());
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator Animation()
    {
        while (_winScreenGroup.alpha < 1)
        {
            _winScreenGroup.alpha += Time.fixedDeltaTime * 2;
            Time.timeScale = 1 - _winScreenGroup.alpha * 0.9f;
            yield return new WaitForFixedUpdate();
        }
        yield return TextAnimation();
    }

    private IEnumerator TextAnimation()
    {
        var text = _winText.text;
        _winText.text = "";
        _winText.gameObject.SetActive(true);
        for (int i = 0; i < text.Length; i++)
        {
            _winText.text = text.Substring(0, i + 1);
            yield return new WaitForFixedUpdate();
        }
        _restartButton.gameObject.SetActive(true);
    }
}
