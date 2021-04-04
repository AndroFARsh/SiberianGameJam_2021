using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public event Action OnStartGame;

    [SerializeField] private ScaleButton startBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(StartGame);
        FindObjectOfType<AudioManager>().PlaySound(AudioManager.Sound.Menu);
    }

    private void StartGame()
    {
        FindObjectOfType<AudioManager>().StopSound(AudioManager.Sound.Menu);
        FindObjectOfType<AudioManager>().PlaySound(AudioManager.Sound.Start);
        OnStartGame?.Invoke();
    }

    private void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
    }
}
