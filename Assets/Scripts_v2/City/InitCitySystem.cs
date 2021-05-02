using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public class InitCitySystem: IEcsRunSystem
    {
        private EcsFilter<Ref<Transform>, CityComponent>.Exclude<Children> filter;
        
        public void Run()
        {
            foreach (var index in filter)
            {
                var entity = filter.GetEntity(index);
                var transform = filter.Get1(index).value;
                
                var places = new Children {value = new List<EcsEntity>()};
                var refs = transform.GetComponentsInChildren<Converter.EntityRef>();
                for (var i = 0; i < refs.Length; ++i)
                {
                    var e = refs[i].Value;
                    if (!e.IsNull() && e.IsAlive())
                    {
                        e.Replace(new Parent(entity));
                        places.value.Add(e);
                    }
                }

                entity.Replace(places);
            }
        }
    }
}