using System.Collections.Generic;
using Leopotam.Ecs;
using UnderwaterCats.CardHolder;
using UnityEngine;

namespace UnderwaterCats
{
    public class RefreshCardPositionSystem: IEcsRunSystem
    {
        private EcsFilter<Position, Parent, CardComponent>.Exclude<Dragging> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var curPosition = filter.Get1(index).value;
                var cardHolder = filter.Get2(index).value;

                var cards = cardHolder.Get<Children>();
                var cardHolderConfig = cardHolder.Get<CardsHolderConfigComponent>();
                var cardHolderPosition = cardHolder.Get<Position>().value;

                var offset = Vector3.left * CalculateOffset(entity, cards.value) * cardHolderConfig.offset;
                var destPosition = cardHolderPosition + offset;

                var position = Vector3.Lerp(curPosition, destPosition, Time.deltaTime * cardHolderConfig.speed);

                entity.Replace(new Position {value = position});
            }
        }
        
        private static float CalculateOffset(EcsEntity entity, List<EcsEntity> list)
        {
            var draggedCount = 0;
            var index = 0;
            var count = 0;
            for (var i = 0; i < list.Count; ++i)
            {
                var entityFromList = list[i];
                if (entityFromList.Has<Dragging>())
                {
                    draggedCount++;
                    continue;
                }

                if (entity == entityFromList)
                {
                    index = i - draggedCount;
                }

                count++;
            }

            return (count - 1) * 0.5f - index;
        }
    }
}