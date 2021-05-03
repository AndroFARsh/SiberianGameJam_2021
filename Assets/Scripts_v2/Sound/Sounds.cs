using System;
using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public class Sounds : Converter
    {
        [Serializable]
        public class GameSound 
        {
            public Sound sound;
            public AudioClip clip;

            [Range(0f, 1f)]
            public float volume;

            [Range(.1f, 3f)]
            public float pitch;

            public bool loop;
        }
        
        [SerializeField] private GameSound[] sounds;

        protected override void OnConvert(EcsWorld world, EcsEntity entity)
        {
            entity.Replace(new Ref<GameObject>(gameObject));
            entity.Replace(new Ref<GameSound[]>(sounds));
        }
    }
}