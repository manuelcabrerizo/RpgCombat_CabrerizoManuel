using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject endPlanel;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        GameManager.onGameEnd += OnShowEndPanel;
    }

    private void OnDestroy()
    {
        GameManager.onGameEnd -= OnShowEndPanel;
    }

    private void OnShowEndPanel(string text)
    { 
        endPlanel.gameObject.SetActive(true);
        this.text.text = text;
    }
}
