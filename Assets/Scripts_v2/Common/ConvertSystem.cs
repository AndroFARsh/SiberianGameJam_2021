using Leopotam.Ecs;

namespace UnderwaterCats
{

    public class ConvertSystem : IEcsRunSystem
    {
        private EcsWorld world;

        public void Run()
        {
            if (Converter.HasToInit)
            {
                var toInit = Converter.ToInit;
                foreach (var entry in toInit)
                {
                    var entity = (entry.Key.TryGetComponent<Converter.EntityRef>(out var @ref))
                        ? @ref.Value
                        : world.NewEntity();
                    foreach (var converter in entry.Value)
                    {
                        converter.Convert(world, entity);
                    }

                    entry.Value.Clear();
                }

                toInit.Clear();
            }
        }
    }
}