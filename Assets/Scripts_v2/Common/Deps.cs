using System.Collections.Generic;
using Leopotam.Ecs;

namespace UnderwaterCats
{
    public struct Ref<T>
    {
        public readonly T value;

        public Ref(T v)
        {
            value = v;
        }
        
        public static implicit operator T(Ref<T> d) => d.value;
        public static implicit operator Ref<T>(T v) => new Ref<T>(v);
        
        public static bool operator ==(Ref<T> a, Ref<T> b)
            => (a.value != null && a.value.Equals(b.value)) || (a.value == null && b.value == null);

        public static bool operator !=(Ref<T> a, Ref<T> b) => !(a == b);
    }
    
    public struct Parent
    {
        public readonly EcsEntity value;
        public Parent(EcsEntity v)
        {
            value = v;
        }
    }
    
    public struct Children
    {
        public List<EcsEntity> value;
    }
}