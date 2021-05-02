using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public class BuildSystem : IEcsRunSystem
    {
        private EcsFilter<OnDropEvent> filter;

        public void Run()
        {
            foreach (var index in filter)
            {
                var dropEvent = filter.Get1(index);

                var placeEntity = dropEvent.target;
                var cardEntity = dropEvent.item;

                var card = cardEntity.Get<CardComponent>().value;
                if (TryApplyCard(placeEntity, card))
                {
                    // placeEntity.Replace<>()   
                }
            }
        }
        
        private bool TryApplyCard(EcsEntity entity, Card card)
        {
            if (!entity.CheckCard(card)) return false;


            var transform = entity.Get<Ref<Transform>>();
            switch (card.Action)
            {
                case ActionType.Build:
                {
                    var view = Object.Instantiate(card.PrefabItemView, transform);

                    entity.Replace(new CardComponent {value = card});
                    entity.Replace(new BuildViewComponent {value = view});

                    return true;
                }
                case ActionType.Destroy:
                {
                    var view = entity.Get<BuildViewComponent>().value;

                    entity.Del<CardComponent>();
                    entity.Del<BuildViewComponent>();

                    Object.Destroy(view);
                    
                    return true;
                }
                default:
                    return false;
            }
        }
    }
}