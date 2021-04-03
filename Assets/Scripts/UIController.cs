using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject WinWindow;
    [SerializeField] private GameObject LoseWindow;

    [SerializeField] private TextMeshProUGUI depthView;

    private void Awake()
    {
        gameManager.OnWin += ShowWin;
        gameManager.OnLose += ShowLose;
    }

    public void UpdateDepth(float depth)
    {
        depthView.text = string.Format ("Depth: {0:0}",  depth);
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

