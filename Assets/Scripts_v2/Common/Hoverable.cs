using System;
using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    struct HoverConfig
    {
        public float hovered;
        public float normal;
        public float speed;
    }

    [RequireComponent(typeof(Collider2D))]
    public class Hoverable: Converter
    {
        [SerializeField] private float scaleOnHover = 1.2f;
        [SerializeField] private float speed = 10f;
        
        private EcsEntity entity;
        protected override void OnConvert(EcsWorld world, EcsEntity e)
        {
            entity = e;
            entity.Replace(new Ref<Transform>(transform));
            entity.Replace(new HoverConfig
            {
                hovered = scaleOnHover,
                normal = transform.localScale.x,
                speed = speed
            });
        }
        
        private void OnMouseEnter()
        {
            if (entity.IsAlive())
            {
                entity.Replace(new OnEnterHoverEvent());
            }
        }   
        
        private void OnMouseExit()
        {
            if (entity.IsAlive())
            {
                entity.Replace(new OnExitHoverEvent());
            }
        }


    }
}