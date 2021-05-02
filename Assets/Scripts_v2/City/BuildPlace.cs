using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    struct BuildViewComponent
    {
        public GameObject value;
    }
    
    struct BuildPlaceConfigComponent
    {
        public int tilt;
        public ItemType allowedItemType;
    }

    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class BuildPlace: Converter
    {
        [SerializeField] private int tilt;
        [SerializeField] private ItemType allowedItemType;
        
        protected override void OnConvert(EcsWorld world, EcsEntity entity)
        {
            entity.Replace(new Ref<SpriteRenderer>(GetComponent<SpriteRenderer>()));
            entity.Replace(new Ref<Collider2D>(GetComponent<Collider2D>()));
            entity.Replace(new BuildPlaceConfigComponent
            {
                tilt = tilt,
                allowedItemType = allowedItemType
            });
        }
    }

    public static class Build
    {
        public static bool CheckCard(this EcsEntity entity, Card card)
        {
            if (!entity.Has<BuildPlaceConfigComponent>())
            {
                return false;
            }

            var buildConfig = entity.Get<BuildPlaceConfigComponent>();
            var notEmpty = entity.Has<BuildViewComponent>();

            switch (card.Action)
            {
                case ActionType.Build:
                    return !notEmpty && card.Type == buildConfig.allowedItemType;
                case ActionType.Destroy:
                    return notEmpty && card.Type == buildConfig.allowedItemType;
                default:
                    Debug.LogError($"Unsupported action type {card.Action}");
                    return false;
            }
        }
    }
}