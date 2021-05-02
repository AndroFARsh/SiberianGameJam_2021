using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public class TiltCitySystem: IEcsRunSystem
    {
        private EcsFilter<CityStatsComponent, TiltConfig, Rotation> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var cityStats = filter.Get1(index);
                var tiltConfig = filter.Get2(index);
                var rotation = filter.Get3(index).value;

                entity.Replace(new Rotation
                {
                    value = new Vector3
                    {
                        x = rotation.x,
                        y = rotation.y,
                        z = cityStats.tilt * tiltConfig.tiltStep
                    }
                });
            }
        }
    }
    
    public class TiltCityViewSystem: IEcsRunSystem
    {
        private EcsFilter<Ref<Transform>, TiltConfig, Rotation> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var transform = filter.Get1(index).value;
                var tiltConfig = filter.Get2(index);
                var rotation = filter.Get3(index).value;
                
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, rotation, Time.deltaTime * tiltConfig.speed);
            }
        }
    }
}