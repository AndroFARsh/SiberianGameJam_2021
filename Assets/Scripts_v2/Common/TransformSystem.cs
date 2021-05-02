using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public class TransformSystem : IEcsRunSystem
    {
        private EcsFilter<Ref<Transform>, Position> filter;

        public void Run()
        {
            foreach (var index in filter)
            {
                var transform = filter.Get1(index).value;
                var position = filter.Get2(index).value;

                transform.position = position;
            }
        }
    }
}