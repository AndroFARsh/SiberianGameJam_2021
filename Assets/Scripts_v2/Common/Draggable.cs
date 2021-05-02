using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    [RequireComponent(typeof(Collider2D))]
    public class Draggable: Converter
    {
        private EcsEntity entity;
        protected override void OnConvert(EcsWorld world, EcsEntity e)
        {
            entity = e;
            entity.Replace(new Ref<Collider2D>(GetComponent<Collider2D>()));
        }
        
        private void OnMouseDown()
        {
            entity.Replace(new OnEnterDragEvent());
        }
        
        private void OnMouseUp()
        {
            entity.Replace(new OnExitDragEvent());
        }
    }
}