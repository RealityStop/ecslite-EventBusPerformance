using EventBusDemo.Components;
using EventBusDemo.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EventBusDemo.Systems
{
	public class DestroyAllEntititesSystem : IEcsRunSystem
	{
		private EcsServiceInject<IEventBus> _eventBus;
		private EcsWorldInject _world;
		private EcsPoolInject<HealthComponent> _healthPool;
		private EcsPoolInject<HealthLowFlag> _healthLowFlagPool;
		private EcsFilterInject<Inc<HealthComponent>> _healthEntityFilter;

		public void Run(EcsSystems systems)
		{
			if (_eventBus.Value.UniqueEvents.Has<DestroyAllEntitiesEvent>())
			{
				foreach (var entityID in _healthEntityFilter.Value)
				{
					//Works by setting all the health to "kill"
					_healthPool.Value.Get(entityID).CurrentHealth = -1;
					if (!_healthLowFlagPool.Value.Has(entityID))
						_healthLowFlagPool.Value.Add(entityID);
				}
				
				_eventBus.Value.UniqueEvents.Del<DestroyAllEntitiesEvent>();				
			}
		}
	}
}