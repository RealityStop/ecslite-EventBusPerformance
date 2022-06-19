using EventBusDemo.Events;
using Leopotam.EcsLite;

namespace EventBusDemo.Systems
{
	public class OptimizedUpdateSystem : IEcsRunSystem
	{
		private readonly EcsServiceInject<IEventBus> _eventBus;

		public void Run(EcsSystems systems)
		{
			ref var update = ref _eventBus.Value.UniqueEvents.Add<UpdateEvent>();
		}
	}
}