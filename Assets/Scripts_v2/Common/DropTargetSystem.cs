using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public class DropTargetEnterSystem : IEcsRunSystem
    {
        private EcsFilter<DropTargetConfig, OnEnterDropTargetEvent> dropFilter;
        private EcsFilter<Dragging> dragFilter;

        public void Run()
        {
            if (dragFilter.IsEmpty())
            {
                return;
            }

            foreach (var index in dropFilter)
            {
                var dropEntity = dropFilter.GetEntity(index);
                var config = dropFilter.Get1(index);

                dropEntity.Replace(new Scale {value = Vector3.one * config.active});
            }
        }
    }

    public class DropTargetSystem : IEcsRunSystem
    {
        private EcsFilter<Ref<Transform>, Scale, DropTargetConfig> filter;

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
    
    public class DropTargetExitSystem : IEcsRunSystem
    {
        private EcsFilter<DropTargetConfig, OnExitDropTargetEvent> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var dropEntity = filter.GetEntity(index);
                var config = filter.Get1(index);
                
                dropEntity.Replace(new Scale {value = Vector3.one * config.normal});
            }
        }
    }
}