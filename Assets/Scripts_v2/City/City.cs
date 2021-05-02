using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public struct CityComponent
    {
    }
    
    public struct CityStatsComponent
    {
        public int tilt;
        public int speed; // engine
        public int firePower; // gun

        public int energyCapacity; // available energy level
        public int energyConsumption; // amount of energy used by all devices
    }

    public struct InitCityStatsComponent
    {
        public int energyCapacity; // available energy level
    }

    public class City: Converter
    {
        [SerializeField] private int energyCapacity; // available energy level
        
        protected override void OnConvert(EcsWorld world, EcsEntity entity)
        {
            entity.Replace(new CityComponent());
            entity.Replace(new Ref<Transform>(transform));
            entity.Replace(new CityStatsComponent());
            entity.Replace(new InitCityStatsComponent
            {
                energyCapacity = energyCapacity,
            });
        }
    }
}