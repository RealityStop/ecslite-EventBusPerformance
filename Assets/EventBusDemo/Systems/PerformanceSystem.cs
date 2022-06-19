using EventBusDemo.Components;
using EventBusDemo.Services;
using Leopotam.EcsLite;

namespace EventBusDemo.Systems
{
	public class PerformanceSystem : IEcsRunSystem
	{
		
		private readonly EcsServiceInject<IPerformanceService> _performanceService;
		private readonly EcsServiceInject<IEventBus> _eventBus;


		public void Run(EcsSystems systems)
		{
			if (_performanceService.Value.StopSpawning)
				_eventBus.Value.UniqueEvents.Add<LowPerformanceUniqueEventComponent>();
			else
				_eventBus.Value.UniqueEvents.Del<LowPerformanceUniqueEventComponent>();
		}
	}
}