using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    struct DropTargetConfig
    {
        public float active;
        public float normal;
        public float speed;
    }
    
    [RequireComponent(typeof(Collider2D))]
    public class DropTarget: Converter
    {
        [SerializeField] private float scaleOnHover = 1.5f;
        [SerializeField] private float speed = 10f;
        
        private EcsEntity entity;
        protected override void OnConvert(EcsWorld world, EcsEntity e)
        {
            entity = e;
            entity.Replace(new Ref<Collider2D>(GetComponent<Collider2D>()));
            entity.Replace(new Ref<Transform>(transform));
            entity.Replace(new DropTargetConfig
            {
                active = scaleOnHover,
                normal = transform.localScale.x,
                speed = speed
            });
        }
        
        private void OnMouseEnter()
        {
            entity.Replace(new OnEnterDropTargetEvent());
        }   
        
        private void OnMouseExit()
        {
            entity.Replace(new OnExitDropTargetEvent());
        }
    }
    
    public struct OnEnterDropTargetEvent
    {
    }
    
    public struct OnExitDropTargetEvent
    {
    }
    
}