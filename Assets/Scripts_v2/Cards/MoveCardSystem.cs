using Leopotam.Ecs;
using UnderwaterCats.CardHolder;
using UnityEngine;

namespace UnderwaterCats
{
    public class MoveCardSystem: IEcsRunSystem
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

                var offset = Vector3.left * ((cards.value.Count - 1) * 0.5f - cards.value.IndexOf(entity)) * cardHolderConfig.offset;
                var destPosition = cardHolderPosition + offset;

                var position = Vector3.Lerp(curPosition, destPosition, Time.deltaTime * cardHolderConfig.speed);

                entity.Replace(new Position {value = position});
            }
        }
    }
}