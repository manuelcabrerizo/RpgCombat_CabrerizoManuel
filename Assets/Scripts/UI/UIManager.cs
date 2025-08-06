using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static event Action<bool> onCreditsPanelShow;

    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        GameManager.onGameEnd += OnShowEndPanel;
        creditsButton.onClick.AddListener(OnCreditsButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    private void OnDestroy()
    {
        GameManager.onGameEnd -= OnShowEndPanel;
        creditsButton.onClick.RemoveListener(OnCreditsButtonClick);
        exitButton.onClick.RemoveListener(OnExitButtonClick);
        mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
    }


    private void OnCreditsButtonClick()
    { 
        creditsCanvas.SetActive(true);
        onCreditsPanelShow?.Invoke(true);
    }

    private void OnExitButtonClick()
    {
        creditsCanvas.SetActive(false);
        onCreditsPanelShow?.Invoke(false);
    }

    private void OnShowEndPanel(string text)
    {
        endCanvas.SetActive(true);
        gameplayCanvas.SetActive(false);
        this.text.text = text;
    }

    private void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
