using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject WinWindow;
    [SerializeField] private GameObject LoseWindow;

    private void Awake()
    {
        gameManager.OnWin += ShowWin;
        gameManager.OnLose += ShowLose;
    }

    private void ShowWin()
    {
        WinWindow.SetActive(true);
    }

    private void ShowLose()
    {
        LoseWindow.SetActive(true);
    }

    private void OnDestroy()
    {
        gameManager.OnWin -= ShowWin;
        gameManager.OnLose -= ShowLose;
    }
}

