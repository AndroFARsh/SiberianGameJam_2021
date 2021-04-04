using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public event Action OnStartGame;

    public static AudioManager audioManager;

    [SerializeField] private ScaleButton startBtn;
    

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager?.PlaySound(AudioManager.Sound.Menu);
        startBtn.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        audioManager?.StopSound(AudioManager.Sound.Menu);
        audioManager?.PlaySound(AudioManager.Sound.Start);
        audioManager?.PlaySound(AudioManager.Sound.Brif);

        OnStartGame?.Invoke();
    }

    private void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
    }
}
