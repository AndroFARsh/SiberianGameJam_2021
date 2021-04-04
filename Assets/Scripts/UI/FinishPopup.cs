using System;
using UnityEngine;
using UnityEngine.UI;

public class FinishPopup : MonoBehaviour
{
    public event Action OnContinueClick;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private Text text;

    [SerializeField] private ScaleButton continueBtn;
    private void Awake()
    {
        continueBtn.onClick.AddListener(StartGame);
    }

    private void OnEnable()
    {
        text.text = "Your depth: " + gameManager.PlayerCity.Depth.ToString();
    }

    private void StartGame()
    {
        OnContinueClick?.Invoke();
    }

    private void OnDestroy()
    {
        continueBtn.onClick.RemoveAllListeners();
    }


}
