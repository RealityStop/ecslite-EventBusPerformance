using EventBusDemo.Events;
using EventBusDemo.Services;
using EventBusDemo.Systems;
using EventBusDemo.Worlds;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.Profiling;

namespace EventBusDemo {
    sealed class EcsStartup : MonoBehaviour {
        EcsSystems _systems;


        private EcsWorld _eventWorld;
        private IEventBus _eventBus;
        private IDemo _demo;


        void Start()
        {
            //We can either construct the EventBus here, or (in Unity) have it as a hosted service so that
            //world observers can set up listeners before our ECS systems have spun up.
            _eventBus = ServiceContainer.Get<IEventBus>();
            
            // register your shared data here, for example:
            // var shared = new Shared ();
            // systems = new EcsSystems (new EcsWorld (), shared);
            _systems = new EcsSystems (new EcsWorld (), ServiceContainer.GetCurrentContainer());
            _systems
                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())
                
                
                .AddWorld (new EcsWorld (), WorldDefinitions.Time)
                .AddWorld (new EcsWorld(), WorldDefinitions.Meta)
                .AddWorld(_eventBus)
                
                
                
                .Add(new MoveSystem())
                .Add(new PerformanceSystem())
                .Add(new SpawnSystem())
                .Add(new LoadAssetSystem())
                
                .Add(new DestroyAllEntititesSystem())
                .Add(new DestroyEntitiesWithLowHealthSystem())
                
                
                .Add(new OptimizedUpdateSystem())
                
                //Add handler for specific event type, if you need enforced order
                .Add(_eventBus.EntityEvents.ProcessorFor<HealthChangedEvent>())
                
                //Process all the remaining events
                .AddAllEvents(_eventBus)
                
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                //.Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Inject()
                .Init ();

                ServiceContainer.Add<EcsSystems>(_systems);

                _demo = ServiceContainer.Get<IDemo>();
        }

        void Update () {
            if (!_demo.CurrentDemoSettings.IsComplete)
            {
                Profiler.BeginSample("ECS systems");
                _systems?.Run();
                Profiler.EndSample();
            }
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                // add here cleanup for custom worlds, for example:
                // _systems.GetWorld ("events").Destroy ();
                foreach(var v in _systems.GetAllNamedWorlds())
                    v.Value.Destroy();
                _systems.GetWorld ().Destroy ();
                _systems = null;
            }
        }
    }
}