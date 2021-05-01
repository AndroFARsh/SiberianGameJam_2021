using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private DeckService deckService;
        
        EcsWorld world;
        EcsSystems systems;

        void Start()
        {
            // void can be switched to IEnumerator for support coroutines.

            world = new EcsWorld();
            systems = new EcsSystems(world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
#endif
            systems
                // register your systems here, for example:
                .Add(new ConvertSystem())
                
                .Add (new DrawCardCheckSystem())
                .Add (new DrawCardSystem())
                
                .Add(new MoveCardSystem())
                
                .Add(new HoverEnterCardSystem())
                .Add(new HoverExitCardSystem())
                .Add(new HoverCardSystem())
                
                .Add(new DragEnterCardSystem())
                .Add(new DragCardSystem())
                .Add(new DragExitCardSystem())
                
                .Add(new TransformSystem())
                
                // register one-frame components (order is important), for example:
                .OneFrame<DrawCardEvent>()
                .OneFrame<OnEnterHoverEvent>()
                .OneFrame<OnExitHoverEvent>()
                .OneFrame<OnEnterDragEvent>()
                .OneFrame<OnExitDragEvent>()
                
                // inject service instances here (order doesn't important), for example:
                .Inject (deckService)
                .Inject (Camera.main)
                // .Inject (new NavMeshSupport ())
                .Init();
        }

        void Update()
        {
            systems?.Run();
        }

        void OnDestroy()
        {
            if (systems != null)
            {
                systems.Destroy();
                systems = null;
                world.Destroy();
                world = null;
            }
        }
    }
}