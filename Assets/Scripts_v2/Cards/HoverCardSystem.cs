using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public class HoverEnterCardSystem: IEcsRunSystem
    {
        private EcsFilter<Scale, OnEnterHoverEvent> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);

                entity.Replace(new Scale {value = Vector3.one * 1.1f});
                entity.Replace(new Hovered());
            }
        }
    }
    
    public class HoverCardSystem : IEcsRunSystem
    {
        private EcsFilter<TransformRef, Scale> filter;

        public void Run()
        {
            foreach (var index in filter)
            {
                var transform = filter.Get1(index).value;
                var newScale = filter.Get2(index).value;
                var curScale = transform.localScale;
                
                transform.localScale = Vector3.Lerp(curScale, newScale, Time.deltaTime * 10);
            }
        }
    }
    
    public class HoverExitCardSystem: IEcsRunSystem
    {
        private EcsFilter<Scale, Hovered, OnExitHoverEvent> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                
                entity.Replace(new Scale {value = Vector3.one});  
                entity.Del<Hovered>();
            }
        }
    }
}