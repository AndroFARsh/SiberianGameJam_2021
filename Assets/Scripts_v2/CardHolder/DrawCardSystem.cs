using Leopotam.Ecs;
using UnderwaterCats.CardHolder;
using UnityEngine;

namespace UnderwaterCats
{

    public class DrawCardSystem : IEcsRunSystem
    {
        private EcsWorld world;
        private IDeckService deckService;
        private EcsFilter<CardsHolderConfigComponent, Children, DrawCardEvent> filter;

        public void Run()
        {
            foreach (var index in filter)
            {
                var holderEntity = filter.GetEntity(index);
                var config = filter.Get1(index);
                var children = filter.Get2(index);
                if (deckService.DrawCard("TODO", out var card, out var view))
                {
                    var transform = view.transform;
                    transform.position = config.spawnPosition;
                    
                    var entity = world.NewEntity();
                    var entityRef = view.gameObject.AddComponent<Converter.EntityRef>();
                    entityRef.Value = entity;
                    
                    entity.Replace(new CardComponent {value = card});
                    entity.Replace(new Parent(holderEntity));
                    children.value.Add(entity);
                    
                    view.Init(card);
                }
                else
                {
                    Debug.LogError("cardView prefab should has CardView component");
                }
            }
        }
    }
}