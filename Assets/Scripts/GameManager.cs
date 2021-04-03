using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private City playerCity;
    [SerializeField] private LoopBackgroundSystem backgroundSystem;
    [SerializeField] private TiltSystem tiltSystem;
    [SerializeField] private ShakeSystem shakeSystem;
    [SerializeField] private List<CardViewDev> cardViews;
    
    private void Awake()
    {
        foreach(var cardView in cardViews)
        {
            cardView.OnAddPart += TryAddPartToPlayerCity;
        }

        playerCity.OnStatsRefreshed += OnStatsRefreshed;
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
        backgroundSystem.SetSpeed(CalculateSpeedBasedOnTilt(stats.Speed, stats.Tilt));
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
