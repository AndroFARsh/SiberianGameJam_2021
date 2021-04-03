using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public event Action OnWin;
    public event Action OnLose;

    [SerializeField] private City playerCity;
    [SerializeField] private LoopBackgroundSystem backgroundSystem;
    [SerializeField] private TiltSystem tiltSystem;
    [SerializeField] private ShakeSystem shakeSystem;

    [SerializeField] private List<CardViewDev> cardViews;
    

    [SerializeField] private List<CardView> cardViews;

    [SerializeField] private TextMeshProUGUI depthView;
    [SerializeField] private float depth;

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
        while (depth > 0)
        {            
            depth -= currentSpeed;

            depthView.text = "Depth: " + (Mathf.Lerp(depth, depth - currentSpeed, Time.deltaTime)).ToString();

            yield return new WaitForSeconds(1f);
        }

        progress = null;

        Win();
    }

    private void Win()
    {
        OnWin?.Invoke();
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

        currentSpeed = CalculateSpeedBasedOnTilt(stats.Speed, stats.Tilt);
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
