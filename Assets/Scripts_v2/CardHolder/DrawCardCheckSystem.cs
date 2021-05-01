using Leopotam.Ecs;
using UnderwaterCats.CardHolder;
using UnityEngine;

namespace UnderwaterCats
{
    // One frame event to draw a card
    struct DrawCardEvent
    {
        // Card holder entity 
        public Card value;
    }

    public class DrawCardCheckSystem: IEcsRunSystem
    {
        private EcsFilter<CardHolderStateComponent, CardsHolderConfigComponent> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var state = filter.Get1(index);
                var config = filter.Get2(index);
                
                // Check if ready to draw the card
                if (state.drawDelay > config.drawDelay && state.numCard < config.maxCard)
                {
                    entity.Replace(new DrawCardEvent());
                    entity.Replace(new CardHolderStateComponent
                    {
                        numCard = state.numCard + 1
                    });
                }
                else
                {
                    entity.Replace(new CardHolderStateComponent
                    {
                        drawDelay = state.drawDelay + Time.deltaTime,
                        numCard = state.numCard
                    });
                }
            }
        }
    }
}