using Leopotam.Ecs;
using UnderwaterCats.CardHolder;
using UnityEngine;

namespace UnderwaterCats
{
    struct Dragging
    {
        public float distance;
        public Vector3 offset;
    }
    
    public class DragEnterCardSystem: IEcsRunSystem
    {
        private Camera camera;
        private EcsFilter<Position, OnEnterDragEvent>.Exclude<Dragging> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var position = filter.Get1(index).value;

                var distance = Vector3.Distance(position, camera.transform.position);
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                var offset = position - ray.GetPoint(distance);

                entity.Replace(new Dragging
                {
                    distance = distance,
                    offset = offset
                });
            }
        }
    }
    
    public class DragCardSystem : IEcsRunSystem
    {
        private Camera camera;
        private EcsFilter<Dragging, Position> filter;
    
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var dragging = filter.Get1(index);
                
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                var position = ray.GetPoint(dragging.distance) + dragging.offset;
                
                entity.Replace(new Position {value = position});
            }
        }
    }
    
    public class DragExitCardSystem: IEcsRunSystem
    {
        private EcsFilter<Dragging, OnExitDragEvent> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                entity.Del<Dragging>();
            }
        }
    }
}