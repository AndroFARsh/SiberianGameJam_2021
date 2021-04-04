using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using System;
using DG.Tweening;
using UnityEngine.Serialization;
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
    [SerializeField] private int defaultSpawnGameEvent = 10;
    private int spawnGameEvent = 10;
    
    [Header("Cards")]
    [SerializeField] private Card[] uniqueCards;
    
    public City EnemyCity;
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
            
            if(!EnemyCity && timeSpawnEnemy == 0)
            {
                if(PlayerCity.transform.position != playerAlonePosition.position)
                {
                    SetCityToPos(PlayerCity, playerAlonePosition.position);
                }
                CreateGameEvent();                
            }
            else
            {
                timeSpawnEnemy--;
            }

            if(timeSpawnEnemy <= 0 && spawnGameEvent == 0)
            {
                CreateGameEvent();
            }
            else if(timeSpawnEnemy <= 0)
            {
                spawnGameEvent--;
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

    private void CreateGameEvent()
    {
        spawnGameEvent = defaultSpawnGameEvent;
                
        if (!EnemyCity)
        {
            var enemy = Instantiate(PlayerCity.gameObject);
            enemy.name = $"{PlayerCity.name}_ENEMY";
            enemy.transform.position = enemySpawnPoint.position;
            enemy.gameObject.SetActive(true);

            EnemyCity = enemy.GetComponent<City>();
            enemyCityAI = enemy.AddComponent<EnemyCity>();
            enemyCityAI.GameManager = this;
            EnemyCity.OnStatsRefreshed += OnStatsRefreshed;

            SetCityToPos(EnemyCity, enemyAttackPosition.position);
            SetCityToPos(PlayerCity, playerDefencePosition.position);

            return;
        }

        float randomEvent = UnityEngine.Random.Range(0f, 3f);

        if(randomEvent < 1)
        {
            AttackRandomCat();
            return;
        } 
        else if(randomEvent < 2)
        {
            enemyCityAI.AttackTarget();
            return;
        }
        else if (randomEvent < 3)
        {
            enemyCityAI.BuildPlace();
            return;
        }
    }

    private void AttackRandomCat()
    {

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

        if (EnemyCity == null)
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
        else if (city == EnemyCity)
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
    
    public Card PickRandomCard(List<CardView> hand, int maxIteration = 5)
    {
        do
        {
            var card = uniqueCards[UnityEngine.Random.Range(0, uniqueCards.Length)];
            if (hand.FindAll(v => v.Card.Action == card.Action && v.Card.Type == card.Type).Count <= 1)
            {
                for (var i = 0; i < PlayerCity.CityPlaces.Count; i++)
                {
                    if (PlayerCity.CityPlaces[i].CheckCard(card) && card.Action != ActionType.Destroy)
                    {
                        return card;
                    }
                }

                if (EnemyCity)
                {
                    for (var i = 0; i < EnemyCity.CityPlaces.Count; i++)
                    {
                        if (PlayerCity.CityPlaces[i].CheckCard(card))
                        {
                            return card;
                        }
                    }
                }
                
                for (var i = 0; i < PlayerCity.CityPlaces.Count; i++)
                {
                    if (PlayerCity.CityPlaces[i].CheckCard(card))
                    {
                        return card;
                    }
                }
            }
        } while (maxIteration-- > 0);
        
        return uniqueCards[UnityEngine.Random.Range(0, uniqueCards.Length)];
    }

    public Card RequestCardOfType(ItemType type, int maxIter = 2)
    {
        do
        {
            var card = uniqueCards[UnityEngine.Random.Range(0, uniqueCards.Length)];
            if (card.Type == type)
            {
                return card;
            }
        } while (maxIter > 0);

        return null;
    }
}
