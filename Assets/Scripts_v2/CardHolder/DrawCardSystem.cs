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
                    entity.Replace(new CardComponent {value = card});
                    entity.Replace(new CardViewComponent {value = view});
                    entity.Replace(new TransformRef {value = transform});
                    
                    entity.Replace(new Position {value = transform.position});
                    entity.Replace(new Rotation {value = transform.eulerAngles});
                    entity.Replace(new Scale {value = transform.localScale});
                    
                    entity.Replace(new Parent {value = holderEntity});
                    children.value.Add(entity);
                    
                    view.Init(entity, card);
                }
                else
                {
                    Debug.LogError("cardView prefab should has CardView component");
                }
            }
        }
    }
}