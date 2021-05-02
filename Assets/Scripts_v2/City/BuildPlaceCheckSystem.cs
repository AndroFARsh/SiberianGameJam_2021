using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    struct DropComponent
    {
        public EcsEntity value;
    }

    struct DragComponent
    {
        public EcsEntity value;
    }

    public class BuildPlaceDragCardEnterSystem : IEcsRunSystem
    {
        private EcsFilter<OnEnterDropTargetEvent> dropFilter;
        private EcsFilter<CardComponent, Dragging> dragFilter;

        public void Run()
        {
            foreach (var dropIndex in dropFilter)
            {
                foreach (var dragIndex in dragFilter)
                {
                    var dragEntity = dragFilter.GetEntity(dragIndex);
                    var card = dragFilter.Get1(dragIndex).value;

                    var dropEntity = dropFilter.GetEntity(dropIndex);
                    if (dropEntity.CheckCard(card))
                    {
                        dropEntity.Replace(new DragComponent {value = dragEntity});
                        dragEntity.Replace(new DropComponent {value = dropEntity});
                    }
                }
            }
        }
    }

    public class BuildPlaceDragCardExitSystem : IEcsRunSystem
    {
        private EcsFilter<DragComponent, OnExitDropTargetEvent> filter;

        public void Run()
        {
            foreach (var index in filter)
            {
                var dropEntity = filter.GetEntity(index);
                var dragEntity = filter.Get1(index).value;

                dropEntity.Del<DragComponent>();

                if (dragEntity.IsAlive())
                {
                    dragEntity.Del<DropComponent>();
                }
            }
        }
    }
}