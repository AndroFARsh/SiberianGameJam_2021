using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats.CardHolder
{
    struct CardHolderComponent
    {
    }

    struct CardsHolderConfigComponent
    {
        public int maxCard;
        public float drawDelay;
        public Vector3 spawnPosition;
        
        public float offset;
        public float speed;
    }

    struct CardHolderStateComponent
    {
        public int numCard;
        public float drawDelay;
    }
    
    struct CardsViewRef
    {
        public EcsEntity value;
    }

    public class CardHolder : Converter
    {
        [SerializeField] private int maxCardNum = 5;
        [SerializeField] private float drawDelay = 0.5f;
        [SerializeField] private Transform cardSpawnPosition;

        [SerializeField] private float offset = 2.5f;
        [SerializeField] private float speed = 0.5f;
        
        protected override void OnConvert(EcsWorld world, EcsEntity entity)
        {
            entity.Replace(new CardHolderComponent());
            entity.Replace(new CardsHolderConfigComponent
            {
                maxCard = maxCardNum,
                drawDelay = drawDelay,
                spawnPosition = cardSpawnPosition.position,
                
                offset = offset,
                speed = speed
            });
            entity.Replace(new CardHolderStateComponent());
            
            entity.Replace(new Position {value = transform.position});
            entity.Replace(new Children {value = new List<EcsEntity>()});
        }
    }
}