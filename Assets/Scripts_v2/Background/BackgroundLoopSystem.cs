using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public class BackgroundLoopSystem : IEcsRunSystem
    {
        private Camera camera;
        private EcsFilter<Ref<Transform>, Ref<Bounds>, LayerConfig> filter;
        private EcsFilter<CityStatsComponent> speedFilter;

        public float GetSpeed() => speedFilter.GetEntitiesCount() > 0 ? speedFilter.Get1(0).speed : 0;

        public void Run()
        {
            var position = camera.transform.position;
            var screenBounds = new Bounds(position,
                camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, position.z)));
            
            foreach (var index in filter)
            {
                var transform = filter.Get1(index).value;
                var bounds = filter.Get2(index).value;
                var layerConfig = filter.Get3(index);
                var speed = GetSpeed();

                position = transform.position;
                bounds.center += position;
                
                var delta = (Time.deltaTime * layerConfig.layerSpeed * speed) * Vector3.down;
                var newPosition = position + delta;

                if (speed > 0 && (screenBounds.center - newPosition).y > (bounds.extents.y + screenBounds.size.y))
                {
                    newPosition.y += bounds.size.y * layerConfig.itemCount;
                }
                else if (speed < 0 && (newPosition - screenBounds.center).y > (bounds.extents.y + screenBounds.size.y))
                {
                    newPosition.y -= bounds.size.y * layerConfig.itemCount;
                }

                transform.position = newPosition;
            }
        }
    }
}