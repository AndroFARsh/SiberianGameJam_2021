using System;
using UnityEngine;

public class FinishPopup : MonoBehaviour
{
    public event Action OnContinueClick;

    [SerializeField] private ScaleButton continueBtn;
    private void Awake()
    {
        continueBtn.onClick.AddListener(StartGame);
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
