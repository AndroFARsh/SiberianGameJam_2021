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
    [SerializeField] private List<CityPlace> cityPlaces = new List<CityPlace>();

    public CityStats CityStats { get; private set; }

    private void Awake()
    {
        //initialize start stats
        RefreshStats();
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
        foreach(var place in cityPlaces)
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
    }
}