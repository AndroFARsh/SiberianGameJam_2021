using Leopotam.Ecs;
using UnderwaterCats.CardHolder;
using UnityEngine;

namespace UnderwaterCats
{
    public class RemoveCardSystem : IEcsRunSystem
    {
        private EcsWorld world;
        private IDeckService deckService;
        private EcsFilter<CardViewComponent, Parent, OnDropEvent> filter;

        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var view = filter.Get1(index).value;
                var cardHolderEntity = filter.Get2(index).value;
                var cardHolderState = cardHolderEntity.Get<CardHolderStateComponent>();
                var cardHolderCards = cardHolderEntity.Get<Children>().value;
                
                cardHolderCards.Remove(entity);
                cardHolderEntity.Replace(new CardHolderStateComponent
                {
                    drawDelay = cardHolderState.drawDelay,
                    numCard = cardHolderState.numCard - 1
                });
                
                Object.Destroy(view.gameObject);
                entity.Destroy();
            }
        }
    }
}