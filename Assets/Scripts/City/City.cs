using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CityStats
{
    public int Tilt;
    public int Speed; //engine
    public int FirePower; //gun

    public int EnergyCapacity; // available energy level
    public int EnergyConsumption; // amount of energy used by all devices
}

public class City : MonoBehaviour
{
    [SerializeField] private CityStats initCityStats;
    public List<CityPlace> CityPlaces = new List<CityPlace>();

    public CityStats CityStats { get; private set; }
    public event Action<CityStats> OnStatsRefreshed; 

    private void Awake()
    {
        //initialize start stats
        RefreshStats();
        
        foreach(var places in CityPlaces)
        {
            places.RequestRefresh += RefreshStats;
        }
    }

    public void RefreshStats()
    {
        var stats = new CityStats
        {    
            Tilt = initCityStats.Tilt,
            Speed = initCityStats.Speed,
            FirePower = initCityStats.FirePower,
            EnergyCapacity = initCityStats.EnergyCapacity,
            EnergyConsumption = initCityStats.EnergyConsumption
        };
        foreach(var place in CityPlaces)
        {
            if (!place.IsEmpty)
            {
                var card = place.Card;
                stats.Tilt += card.Tilt * place.TiltFactor;
                switch (card.Type)
                {
                    case ItemType.Balloon:
                    case ItemType.Engine:
                        stats.Speed += card.Value;
                        stats.EnergyConsumption += card.Power;
                        break;
                    case ItemType.Generator:
                        stats.EnergyCapacity += card.Value;
                        break;
                    case ItemType.Gun:
                        stats.FirePower += card.Value;
                        stats.EnergyConsumption += card.Power;
                        break;
                } 
            }
        }
        CityStats = stats;
        
        OnStatsRefreshed?.Invoke(stats);
    }
}