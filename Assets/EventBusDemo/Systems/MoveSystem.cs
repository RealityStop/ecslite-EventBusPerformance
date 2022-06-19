using EventBusDemo.Components;
using EventBusDemo.Events;
using EventBusDemo.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EventBusDemo.Systems
{
	public class MoveSystem : IEcsRunSystem
	{
		private readonly EcsServiceInject<IDemo> _demoService;
		private readonly EcsServiceInject<ILogService> _logService;
		private readonly EcsServiceInject<ITimeService> _timeService;

		private readonly EcsPoolInject<PositionComponent> _positionPool;
		private readonly EcsPoolInject<HealthComponent> _healthPool;
		private readonly EcsServiceInject<IDisplayService> _displayService;
		private EcsServiceInject<IEventBus> _eventBus;

		private EcsPoolInject<VelocityComponent> _velocityPool;
		private EcsFilterInject<Inc<PositionComponent, VelocityComponent>> _positionFilter;

		
		//Flag Components only
		private readonly EcsPoolInject<HealthChangedFlag> _healthChangedFlagPool;
		
		
		
		public void Run(EcsSystems systems)
		{
			//In reality, we'd want to disable this if there was no position changes.
			//But then we don't really learn anything about the impact of the events.   We
			//want this logic processing, even if nobody is listening.
			//if (_demoService.Value.CurrentDemoSettings.PositionChanges == DemoEventStyle.None)
			//	return;
			
			foreach (var positionEntity in _positionFilter.Value)
			{
				ref var positionComponent =  ref _positionPool.Value.Get(positionEntity);
				ref var velocityComponent = ref _velocityPool.Value.Get(positionEntity);

				var positionComponentPosition = positionComponent.Position;
				positionComponentPosition += velocityComponent.CurrentVelocity * _timeService.Value.DeltaTime;
				
				
				//Check Extents
				if (positionComponentPosition.y < _displayService.Value.Top)
				{
					positionComponentPosition = new Vector2(positionComponentPosition.x, _displayService.Value.Top);
					velocityComponent.CurrentVelocity.y = Mathf.Abs(velocityComponent.CurrentVelocity.y);
					DealDamage(positionEntity);
				}

				if (positionComponentPosition.y > _displayService.Value.Bottom)
				{
					positionComponentPosition = new Vector2(positionComponentPosition.x, _displayService.Value.Bottom);
					velocityComponent.CurrentVelocity.y = -1f * Mathf.Abs(velocityComponent.CurrentVelocity.y);
					DealDamage(positionEntity);
				}

				if (positionComponentPosition.x < _displayService.Value.Left)
				{
					positionComponentPosition = new Vector2(_displayService.Value.Left, positionComponentPosition.y);
					velocityComponent.CurrentVelocity.x = Mathf.Abs(velocityComponent.CurrentVelocity.x);
					DealDamage(positionEntity);
				}

				if (positionComponentPosition.x > _displayService.Value.Right)
				{
					positionComponentPosition = new Vector2(_displayService.Value.Right, positionComponentPosition.y);
					velocityComponent.CurrentVelocity.x = -1f * Mathf.Abs(velocityComponent.CurrentVelocity.x);
					DealDamage(positionEntity);
				}
				
				positionComponent.Position = positionComponentPosition;
			}
		}


		private void DealDamage(int positionEntity)
		{
			if (_healthPool.Value.Has(positionEntity))
			{
				ref var health = ref _healthPool.Value.Get(positionEntity);
				var currentHealth = health.CurrentHealth -1;
				

				if (_demoService.Value.CurrentDemoSettings.HealthChanges == DemoEventStyle.None)
					return;
				
				if (_demoService.Value.CurrentDemoSettings.HealthChanges == DemoEventStyle.Event)
				{
					ref var changed = ref _eventBus.Value.EntityEvents.Add<HealthChangedEvent>(
						_healthPool.Value.GetWorld().PackEntityWithWorld(positionEntity));
					changed.newHealth = currentHealth;
					changed.newHealthPercent = currentHealth / (float)health.MaxHealth;
				}
				else if (_demoService.Value.CurrentDemoSettings.HealthChanges == DemoEventStyle.FlagComponents)
				{
					_eventBus.Value.FlagComponents.Add<HealthChangedFlag>(positionEntity, _healthChangedFlagPool.Value);
				}
				
				if (currentHealth <= 0)
					_eventBus.Value.FlagComponents.Add<HealthLowFlag>(_healthPool.Value.GetWorld().PackEntityWithWorld(positionEntity));

				health.CurrentHealth = currentHealth;
			}
		}
	}
}