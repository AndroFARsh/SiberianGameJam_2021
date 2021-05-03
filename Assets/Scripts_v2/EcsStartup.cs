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
            var soundManager = new SoundServiceSystem();
            
            world = new EcsWorld();
            systems = new EcsSystems(world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
#endif
            systems
                // register your systems here, for example:
                .Add(new ConvertSystem())
                
                .Add(new RefreshCardPositionSystem())
                
                .Add (new DrawCardCheckSystem())
                .Add (new DrawCardSystem())
                
                .Add(new HoverEnterCardSystem())
                .Add(new HoverExitCardSystem())
                .Add(new HoverCardSystem())
                
                .Add(new DragEnterCardSystem())
                .Add(new DragCardSystem())
                .Add(new DragExitCardSystem())
                
                .Add(new BuildPlaceDragCardEnterSystem())
                .Add(new BuildPlaceDragCardExitSystem())
                
                .Add(new DropTargetEnterSystem())
                .Add(new DropTargetExitSystem())
                .Add(new DropTargetSystem())
                
                .Add(new InitCitySystem())
                .Add(new RefreshCityStatsSystem())
                .Add(new TiltCitySystem())
                .Add(new TiltCityViewSystem())
                .Add(new BackgroundLoopSystem())
                
                .Add(new TransformSystem())
                
                .Add(new BuildSystem())
                .Add(new RemoveCardSystem())
                .Add(soundManager)
                
                // register one-frame components (order is important), for example:
                .OneFrame<DrawCardEvent>()
                
                .OneFrame<OnEnterHoverEvent>()
                .OneFrame<OnExitHoverEvent>()
                
                .OneFrame<OnEnterDragEvent>()
                .OneFrame<OnExitDragEvent>()
                
                .OneFrame<OnEnterDropTargetEvent>()
                .OneFrame<OnExitDropTargetEvent>()
                
                .OneFrame<OnDropEvent>()
                
                // inject service instances here (order doesn't important), for example:
                .Inject(deckService)
                .Inject(Camera.main)
                .Inject(soundManager)
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