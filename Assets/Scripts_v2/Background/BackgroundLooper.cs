using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    struct LayerConfig
    {
        public float layerSpeed;
        public int itemCount;
    }

    public class BackgroundLooper : Converter
    {
        [SerializeField] private SpriteRenderer[] items;
        [SerializeField] private float layerSpeed;
        
        protected override void OnConvert(EcsWorld world, EcsEntity entity)
        {
            var children = new Children {value = new List<EcsEntity>()};
            
            for (var i=0; i<items.Length; ++i)
            {
                var renderer = items[i];
                var transform = renderer.transform;
                
                var e = world.NewEntity();
                
                e.Replace(new Ref<Transform>(transform));
                e.Replace(new Ref<Bounds>(renderer.bounds));
                e.Replace(new Ref<SpriteRenderer>(renderer));
                e.Replace(new Parent(entity));
                e.Replace(new LayerConfig
                {
                    layerSpeed = layerSpeed,
                    itemCount = items.Length
                });

                children.value.Add(e);        
            }
            entity.Replace(children);
        }
    }
}