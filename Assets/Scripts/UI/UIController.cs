using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Turn of/off all menu states (very lazy variant)
/// </summary>
public class UIController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private MainMenu mainWindow;

    [SerializeField] private GameObject gameWindow;
    [SerializeField] private Image fillEnergyBar;

    [SerializeField] private FinishPopup winWindow;
    [SerializeField] private FinishPopup loseWindow;

    City cap;

    private void Awake()
    {
        gameManager.OnWin += ShowWin;
        gameManager.OnLose += ShowLose;

        mainWindow.OnStartGame += ShowGameUI;
        loseWindow.OnContinueClick += ShowMainMenu;
        winWindow.OnContinueClick += ShowMainMenu;
        gameManager.PlayerCity.OnStatsRefreshed += UpdateBar;

        UpdateBar(cap, gameManager.PlayerCity.CityStats);
    }

    private void UpdateBar(City city , CityStats stats)
    {
        if(stats.EnergyCapacity == 0)
        {
            fillEnergyBar.DOFillAmount(0, 1f);
            return;
        }

        float to = (float)(stats.EnergyCapacity - stats.EnergyConsumption) / (float)stats.EnergyCapacity;

        fillEnergyBar.DOFillAmount(to, 1f);
    }


    private void ShowMainMenu()
    {
        mainWindow.gameObject.SetActive(true);

        gameWindow.gameObject.SetActive(false);
        winWindow.gameObject.SetActive(false);
        loseWindow.gameObject.SetActive(false);
    }

    private void ShowGameUI()
    {
        gameWindow.gameObject.SetActive(true);

        mainWindow.gameObject.SetActive(false);
    }

    private void ShowWin()
    {
        winWindow.gameObject.SetActive(true);

        gameWindow.gameObject.SetActive(false);
    }

    private void ShowLose()
    {
        loseWindow.gameObject.SetActive(true);

        gameWindow.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {

        gameManager.OnWin -= ShowWin;
        gameManager.OnLose -= ShowLose;

        mainWindow.OnStartGame -= ShowGameUI;
        loseWindow.OnContinueClick -= ShowMainMenu;
        winWindow.OnContinueClick -= ShowMainMenu;


        gameManager.PlayerCity.OnStatsRefreshed -= UpdateBar;
    }

    private void OnValidate()
    {
        if (!winWindow)
        {
            Debug.LogError("No win window");
        }

        if (!loseWindow)
        {
            Debug.LogError("No lose window");
        }

        if (!mainWindow)
        {
            Debug.LogError("No Main window");
        }
    }
}

