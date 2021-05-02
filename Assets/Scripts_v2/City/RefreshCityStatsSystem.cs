using System.Collections.Generic;
using Leopotam.Ecs;

namespace UnderwaterCats
{
    public class RefreshCityStatsSystem: IEcsRunSystem
    {
        private EcsFilter<InitCityStatsComponent, Children, CityComponent> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var initCityStats = filter.Get1(index);
                var cityPlaces = filter.Get2(index).value;

                entity.Replace(RefreshStats(initCityStats, cityPlaces));
            }
        }

        private static CityStatsComponent RefreshStats(InitCityStatsComponent initCityStats, List<EcsEntity> cityPlaces)
        {
            var stats = new CityStatsComponent
            {
                energyCapacity = initCityStats.energyCapacity,
            };
            foreach (var place in cityPlaces)
            {
                if (place.Has<CardComponent>())
                {
                    var placeConfig = place.Get<BuildPlaceConfigComponent>();
                    var card = place.Get<CardComponent>().value;
                    stats.tilt += card.Tilt * placeConfig.tilt;
                    switch (card.Type)
                    {
                        case ItemType.Balloon:
                        case ItemType.Engine:
                            stats.speed += card.Value;
                            stats.energyConsumption += card.Power;
                            break;
                        case ItemType.Generator:
                            stats.energyCapacity += card.Value;
                            break;
                        case ItemType.Gun:
                            stats.firePower += card.Value;
                            stats.energyConsumption += card.Power;
                            break;
                    }
                }
            }

            if (stats.energyCapacity <= stats.energyConsumption)
            {
                stats.firePower = 0;
                stats.speed = 0;
                foreach (var place in cityPlaces)
                {
                    if (place.Has<CardComponent>())
                    {
                        var card = place.Get<CardComponent>().value;
                        if (card.Type == ItemType.Balloon)
                        {
                            stats.speed += card.Value;
                        }
                    }
                }
            }

            return stats;
        }
    }
}