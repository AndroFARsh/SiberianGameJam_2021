using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public abstract class Converter: MonoBehaviour
    {
        private static readonly object mutex = new object();
        private static bool hasToInit;
        private static Dictionary<GameObject, HashSet<Converter>> toInit = new Dictionary<GameObject, HashSet<Converter>>();

        public static bool HasToInit => hasToInit;

        protected virtual bool ShouldCleanUp =>
            #if UNITY_EDITOR && DEBUG
                false;
            #else
                true;
            #endif
            

        public static Dictionary<GameObject, HashSet<Converter>> ToInit
        {
            get
            {
                lock (mutex)
                {
                    hasToInit = false;
                    var tmp = toInit;
                    toInit = new Dictionary<GameObject, HashSet<Converter>>();
                    return tmp;
                }
            }
        }

        private void Awake()
        {
            lock (mutex)
            {
                hasToInit = true;
                if (!toInit.ContainsKey(gameObject))
                {
                    toInit.Add(gameObject, new HashSet<Converter>());
                }

                toInit[gameObject].Add(this);
            }
        }

        public void Convert(EcsWorld world, EcsEntity entity)
        {
            if (!TryGetComponent<EntityRef>(out var @ref))
            {
                var entityRef = gameObject.AddComponent<EntityRef>();
                entityRef.value = entity;
            }

            OnConvert(world, entity);

            if (ShouldCleanUp)
            {
                Destroy(this);
            }
        }

        protected abstract void OnConvert(EcsWorld world, EcsEntity entity);
        
        public class EntityRef : MonoBehaviour {
        
            internal EcsEntity value;
            
            public bool IsAlive => value.IsAlive();
            
            public EcsEntity Value => value.IsAlive() ? value : EcsEntity.Null;
            
            public static implicit operator EcsEntity(EntityRef v) => v.value;
        }
    }
}