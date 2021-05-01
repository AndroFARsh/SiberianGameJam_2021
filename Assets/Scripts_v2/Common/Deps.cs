using System.Collections.Generic;
using Leopotam.Ecs;

namespace UnderwaterCats
{
    public struct Parent
    {
        public EcsEntity value;
    }
    
    public struct Children
    {
        public List<EcsEntity> value;
    }
}