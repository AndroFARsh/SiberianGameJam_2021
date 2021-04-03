using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using System;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public event Action OnWin;
    public event Action OnLose;

    [SerializeField] private UIController UIController;
    [SerializeField] private LoopBackgroundSystem backgroundSystem;

    [Header("Player")]
    [SerializeField] private City playerCity;
    [SerializeField] private TiltSystem tiltSystem;
    [SerializeField] private ShakeSystem shakeSystem;
    [SerializeField] private Transform playerAlonePosition;
    [SerializeField] private Transform playerDefencePosition;

    [Header("Card Settings")]
    [SerializeField] private List<CardViewDev> cardViews;

    [Header("Enemy")]
    [SerializeField] private EnemyCity prefabEnemyCity;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private Transform enemyAttackPosition;

    [Header("Game Global Parameters")]
    [Header("Meters")]
    [SerializeField] private float depth = 1000;
    [SerializeField] private float depthSpawnEnemy = 900;

    [Header("Seconds")]
    [SerializeField] private int spawnEnemyDelay = 15;

    private EnemyCity enemyCity;
    
    private float currentSpeed;

    Coroutine progress;

    private void Awake()
    {
        foreach(var cardView in cardViews)
        {
            cardView.OnAddPart += TryAddPartToPlayerCity;
        }

        playerCity.OnStatsRefreshed += OnStatsRefreshed;

        progress = StartCoroutine(Progress());
    }

    private IEnumerator Progress()
    {
        while (depth > 0 && playerCity != null)
        {
            if (!enemyCity)
            {
                spawnEnemyDelay--;
                if(playerCity.transform.position != playerAlonePosition.position)
                {
                    SetCityToPos(playerCity, playerAlonePosition.position);
                }
            }

            depth -= currentSpeed;

            CreateGameEvent();

            yield return new WaitForSeconds(1f);
        }

        progress = null;

        if(depth > 0)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    private void CreateGameEvent()
    {
        if(depth > depthSpawnEnemy && spawnEnemyDelay <= 0)
        {
            if (!enemyCity)
            {
                var enemy = Instantiate(prefabEnemyCity);
                enemy.transform.position = enemySpawnPoint.position;
                enemy.gameObject.SetActive(true);

                enemyCity = enemy.GetComponent<EnemyCity>();
                enemyCity.Target = playerCity;

                SetCityToPos(enemyCity, enemyAttackPosition.position);
                SetCityToPos(playerCity, playerDefencePosition.position);
            }
        }
    }

    private void SetCityToPos(City city, Vector3 pos)
    {
        city.transform.DOMove(pos, 3f);
    }

    private void Win()
    {
        OnWin?.Invoke();
    }
    private void Lose()
    {
        OnLose?.Invoke();
    }

    private void TryAddPartToPlayerCity(Card card, CityPlace place)
    {
        if (place.TryApplyCard(card))
        {
            playerCity.RefreshStats();
            
            OnStatsRefreshed(playerCity.CityStats);
        }
    }
    private void OnStatsRefreshed(CityStats stats)
    {
        currentSpeed = depth > 0 ? CalculateSpeedBasedOnTilt(stats.Speed, stats.Tilt) : 0;
         
        backgroundSystem.SetSpeed(currentSpeed);

        tiltSystem.SetTilt(stats.Tilt);
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
            shakeSystem.StartShake(tiltAbs);
            return 0.0f;
        }

        if(shakeSystem.IsShaking)
            shakeSystem.StopShake();

        if (tiltAbs >= 1)
        {
            speed *= 0.5f;
        }
        
        return  speed;
    }
}
