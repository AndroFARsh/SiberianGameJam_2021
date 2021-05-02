using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    
    public struct OnEnterHoverEvent
    {
    }
    
    public struct Hovered
    {
    }
    
    public struct OnExitHoverEvent
    {
    }
    
    public class HoverEnterCardSystem: IEcsRunSystem
    {
        private EcsFilter<HoverConfig, OnEnterHoverEvent> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var config = filter.Get1(index);
                
                entity.Replace(new Scale {value = Vector3.one * config.hovered});
                entity.Replace(new Hovered());
            }
        }
    }
    
    public class HoverCardSystem : IEcsRunSystem
    {
        private EcsFilter<Ref<Transform>, Scale, HoverConfig> filter;

        public void Run()
        {
            foreach (var index in filter)
            {
                var transform = filter.Get1(index).value;
                var newScale = filter.Get2(index).value;
                var config = filter.Get3(index);
                var curScale = transform.localScale;
                
                transform.localScale = Vector3.Lerp(curScale, newScale, Time.deltaTime * config.speed);
            }
        }
    }
    
    public class HoverExitCardSystem: IEcsRunSystem
    {
        private EcsFilter<HoverConfig, Hovered, OnExitHoverEvent> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var config = filter.Get1(index);
                
                entity.Replace(new Scale {value = Vector3.one * config.normal});  
                entity.Del<Hovered>();
            }
        }
    }
}