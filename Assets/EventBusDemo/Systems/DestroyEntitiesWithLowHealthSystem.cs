using EventBusDemo.Components;
using EventBusDemo.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EventBusDemo.Systems
{
	public class DestroyEntitiesWithLowHealthSystem : IEcsRunSystem
	{
		private readonly EcsServiceInject<IViewService> _viewService;
		private readonly EcsWorldInject _world;
		private EcsPoolInject<HealthComponent> _healthPool;
		private EcsFilterInject<Inc<HealthLowFlag>> _healthEntityFilter;
		
		public void Run(EcsSystems systems)
		{
			foreach (var healthEntity in _healthEntityFilter.Value)
			{
				if (_healthPool.Value.Get(healthEntity).CurrentHealth <= 0)
				{
					Destroy(healthEntity);
				}
			}
		}


		private void Destroy(int healthEntity)
		{
			_viewService.Value.Destroy(_world.Value.PackEntityWithWorld(healthEntity));
			_world.Value.DelEntity(healthEntity);
		}
	}
}