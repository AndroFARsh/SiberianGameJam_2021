using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public event Action OnWin;
    public event Action OnLose;

    private event Action OnRefreshPlayerStats;

    [SerializeField] private UIController UIController;
    [SerializeField] private LoopBackgroundSystem backgroundLeft;
    [SerializeField] private LoopBackgroundSystem backgroundRight;

    [Header("Player")]
    public City PlayerCity;
    [SerializeField] private Transform playerAlonePosition;
    [SerializeField] private Transform playerDefencePosition;
    
    [Header("Enemy")]
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private Transform enemyAttackPosition;
    [SerializeField] private Image divider;
    [SerializeField] private float depthSpawnEnemy;
    
    [Header("Game Global Parameters")]
    [SerializeField] private float depth = 1000;
    [SerializeField] private float timeSpawnEnemy = 900;


    [Header("Conditions Progress")]
    [SerializeField] private float totalTime = 90;

    private float currentPlayerSpeed;

    [Header("Seconds")]
    [SerializeField] private int spawnEnemyDelay = 15;
    
    private City enemyCity;
    private EnemyCity enemyCityAI;
   
    Coroutine progress;

    private void Awake()
    {
        PlayerCity.OnStatsRefreshed += OnStatsRefreshed;

        PlayerCity.Depth = depth;

        progress = StartCoroutine(Progress());
    }

    private IEnumerator Progress()
    {
        while (depth > 0 && totalTime > 0 && PlayerCity != null)
        {
            PlayerCity.Depth -= currentPlayerSpeed;

            if (!enemyCity)
            {
                if(PlayerCity.transform.position != playerAlonePosition.position)
                {
                    SetCityToPos(PlayerCity, playerAlonePosition.position);
                }

                CreateEnemy();
            }

            yield return new WaitForSeconds(1f);
        }

        progress = null;

        if(PlayerCity.Depth <= 0)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    private void CreateEnemy()
    {
        if(depth > depthSpawnEnemy && spawnEnemyDelay <= 0)
        {
            if (!enemyCity)
            {
                var enemy = Instantiate(PlayerCity.gameObject);
                enemy.name = $"{PlayerCity.name}_ENEMY";
                enemy.transform.position = enemySpawnPoint.position;
                enemy.gameObject.SetActive(true);

                enemyCity = enemy.GetComponent<City>();
                enemyCityAI = enemy.AddComponent<EnemyCity>();
                enemyCityAI.Target = PlayerCity;
                enemyCityAI.OnStatsRefreshed += OnStatsRefreshed;
                enemyCity.OnStatsRefreshed += OnStatsRefreshed;

                SetCityToPos(enemyCity, enemyAttackPosition.position);
                SetCityToPos(PlayerCity, playerDefencePosition.position);
            }
        }
    }

    private void SetCityToPos(City city, Vector3 pos)
    {
        city.transform.DOMove(pos, 3f)
            .OnComplete(() =>
            {
                if (divider != null)
                {
                    divider.DOFade(1, 0.2f);
                }
            });
    }

    private void Win()
    {
        OnWin?.Invoke();
    }
    private void Lose()
    {
        OnLose?.Invoke();
    }


    private void OnStatsRefreshed(City city, CityStats stats)
    {
        var currentSpeed = depth > 0 ? CalculateSpeedBasedOnTilt(stats.Speed, stats.Tilt) : 0;

        if (enemyCity == null)
        {
            currentPlayerSpeed = currentSpeed;
            
            backgroundLeft.SetSpeed(currentSpeed);
            backgroundRight.SetSpeed(currentSpeed);
        } 
        else if (city == PlayerCity)
        {
            currentPlayerSpeed = currentSpeed;
            
            backgroundLeft.SetSpeed(currentSpeed);
        }
        else if (city == enemyCity)
        {
            backgroundRight.SetSpeed(currentSpeed);
        }
    }

    private float CalculateSpeedBasedOnTilt(float speed, int tilt)
    {
        var tiltAbs = Mathf.Abs(tilt);
        
        if (tiltAbs >= 3)
        {
            //TODO: no negative progress: return -0.5f;
        }
        
        if (tiltAbs >= 2)
        {
            return 0.0f;
        }

        if (tiltAbs >= 1)
        {
            speed *= 0.5f;
        }
        
        return speed;
    }
}
