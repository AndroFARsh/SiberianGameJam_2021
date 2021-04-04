using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public event Action OnStartGame;

    [SerializeField] private ScaleButton startBtn;
    
    private AudioManager manager;


    private void Awake()
    {
        startBtn.onClick.AddListener(StartGame);
        manager = FindObjectOfType<AudioManager>();
        manager?.PlaySound(AudioManager.Sound.Menu);
    }

    private void StartGame()
    {
        manager?.StopSound(AudioManager.Sound.Menu);
        manager?.PlaySound(AudioManager.Sound.Start);

        OnStartGame?.Invoke();
    }

    private void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
    }
}
