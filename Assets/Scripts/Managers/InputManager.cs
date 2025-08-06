using System;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static event Action onMoveUp;
    public static event Action onMoveDown;
    public static event Action onMoveLeft;
    public static event Action onMoveRight;

    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private bool isPaused = false;

    private void Awake()
    {
        upButton.onClick.AddListener(OnUpButtonClick);
        downButton.onClick.AddListener(OnDownButtonClick);
        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);
        UIManager.onCreditsPanelShow += OnCreditsPanelShow;

        isPaused = false;
    }

    private void OnDestroy()
    {
        upButton.onClick.RemoveListener(OnUpButtonClick);
        downButton.onClick.RemoveListener(OnDownButtonClick);
        leftButton.onClick.RemoveListener(OnLeftButtonClick);
        rightButton.onClick.RemoveListener(OnRightButtonClick);
        UIManager.onCreditsPanelShow -= OnCreditsPanelShow;
    }

    private void Update()
    {
        if (isPaused)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            onMoveUp?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            onMoveDown?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            onMoveLeft?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            onMoveRight?.Invoke();
        }
    }

    private void OnUpButtonClick()
    {
        onMoveUp?.Invoke();
    }

    private void OnDownButtonClick()
    {
        onMoveDown?.Invoke();
    }

    private void OnLeftButtonClick()
    {
        onMoveLeft?.Invoke();
    }

    private void OnRightButtonClick()
    {
        onMoveRight?.Invoke();
    }

    private void OnCreditsPanelShow(bool show)
    {
        if (show)
        {
            isPaused = true;
            Time.timeScale = 0.0f;
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1.0f;
        }
    }
}
