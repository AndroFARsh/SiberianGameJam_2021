using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public event Action OnStartGame;

    [SerializeField] private ScaleButton startBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        OnStartGame?.Invoke();
    }

    private void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
    }
}
