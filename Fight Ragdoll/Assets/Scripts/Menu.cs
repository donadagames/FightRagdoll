using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] CanvasGroup fadeCanvasGroup;
    [SerializeField] GameObject startPanel;


    private void Start()
    {
        fadeCanvasGroup.alpha = 1f;
        fadeCanvasGroup.DOFade(0, 2).OnComplete(() => fadeCanvasGroup.gameObject.SetActive(false));
    }

    public void OnStartButton()
    {
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.DOFade(1, 2).OnComplete(() =>
        {
            startPanel.SetActive(false);
            fadeCanvasGroup.DOFade(0, 2).OnComplete(() =>
            {
                fadeCanvasGroup.gameObject.SetActive(false);
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            });
        });
    }
}
