using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    struct TiltConfig
    {
        public float tiltStep;
        public float speed;
    }
    
    public class Tilt: Converter
    {
        [SerializeField] private float tiltStep = 15;
        [SerializeField] private float speed = 5;

        protected override void OnConvert(EcsWorld world, EcsEntity entity)
        {
            entity.Replace(new Rotation {value = transform.eulerAngles});
            entity.Replace(new Ref<Transform>(transform));
            entity.Replace(new TiltConfig
            {
                tiltStep = tiltStep,
                speed = speed
            });
        }
    }
}