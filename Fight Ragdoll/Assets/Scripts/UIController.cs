using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI maxCollectableText;
    [SerializeField] CanvasGroup fadeCanvasGroup;
    [SerializeField] GameObject playAgainPanel;
    [SerializeField] TimerController timerController;

    Player player;

    private void Start()
    {
        fadeCanvasGroup.alpha = 1f;
        fadeCanvasGroup.DOFade(0, 1).OnComplete(() => fadeCanvasGroup.gameObject.SetActive(false));
        player = FindAnyObjectByType<Player>();
        UpdateMoney(player.ragdollsCount * 10);
        timerController.OnTimerCompleted += OnTimerComplete;
        UpdateMaxCollectable();
    }

    public void UpdateMoney(int value) => moneyText.text = $"${value},00";
    public void UpdateMaxCollectable() => maxCollectableText.text = $"{player.ragdollsCount}/{player.pileController.maxNumberOfSlots}";
    public void UpdateTimerText(string time) => timeText.text = time;

    private void OnTimerComplete(object sender, TimerController.OnTimerCompletedEventHandler e)
    {
        playAgainPanel.SetActive(true);
    }


    public void ExitButton()
    {
        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0f;

        fadeCanvasGroup.DOFade(1, 2).OnComplete(() =>
        {
            fadeCanvasGroup.gameObject.SetActive(false);
            Application.Quit();
        });
    }


    public void OnPlyAgainButton()
    {
        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0f;

        fadeCanvasGroup.DOFade(1, 2).OnComplete(() =>
        {
            fadeCanvasGroup.gameObject.SetActive(false);
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        });
    }
}
