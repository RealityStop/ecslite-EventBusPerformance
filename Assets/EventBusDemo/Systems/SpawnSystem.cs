using EventBusDemo.Components;
using EventBusDemo.Events;
using EventBusDemo.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EventBusDemo.Systems
{
	public class SpawnSystem : IEcsRunSystem
	{
		private float _spawnDelay = 0.01f;
		private float _spawnBatchSize = 10f;
		private float _lastSpawnTime;
		
		
		private readonly EcsServiceInject<IEventBus> _eventbus;
		private readonly EcsServiceInject<IPerformanceService> _performanceService;
		private readonly EcsServiceInject<IDisplayService> _displayService;
		private readonly EcsServiceInject<IDemo> _demoService;
		private readonly EcsPoolInject<AssetComponent> _assetPool;
		private readonly EcsPoolInject<DemoPrefabComponent> _prefabPool;
		private readonly EcsPoolInject<PositionComponent> _positionPool;
		private readonly EcsPoolInject<VelocityComponent> _velocityPool;
		private readonly EcsPoolInject<HealthComponent> _healthPool;

		readonly EcsWorldInject _world;


		public void Run(EcsSystems systems)
		{
			
			
			if (Time.time > _lastSpawnTime + _spawnDelay)
				if (!_eventbus.Value.UniqueEvents.Has<LowPerformanceUniqueEventComponent>())
				{
					var spawnCount = _spawnBatchSize * _demoService.Value.CurrentDemoSettings.SpawnRateMultiplier;
					if (_performanceService.Value.HighFPSNotification)
						spawnCount = spawnCount * 10;
					for (int i = 0; i < spawnCount; i++)
					{
						if (_performanceService.Value.EntityCount >= _demoService.Value.MaxEntityCount)
							return;
						Spawn();
					}
				}
		}


		public Vector2 RotateVector(Vector2 v, float angle)
		{
			float _x = v.x*Mathf.Cos(angle) - v.y*Mathf.Sin(angle);
			float _y = v.x*Mathf.Sin(angle) + v.y*Mathf.Cos(angle);
			return new Vector2(_x,_y);  
		}

		private void Spawn()
		{
			var newEntity = _world.Value.NewEntity();
			
				_prefabPool.Value.Add(newEntity);
			//prefab.prefab = _demoService.Value.CurrentPrefab;
			//ref var asset = ref _assetPool.Value.Add(newEntity);
			//asset.Name = "Circle Polling";

			ref var position = ref _positionPool.Value.Add(newEntity);
			position.Position = new Vector2(Random.Range(_displayService.Value.Left, _displayService.Value.Right),
				Random.Range(_displayService.Value.Bottom, _displayService.Value.Top));

			ref var velocity = ref _velocityPool.Value.Add(newEntity);
			velocity.CurrentVelocity = RotateVector(Vector2.right * Random.Range(0.5f, 2f), Random.Range(-2*Mathf.PI, 2 * Mathf.PI));

			ref var health = ref _healthPool.Value.Add(newEntity);
			health.MaxHealth = 10;
			health.CurrentHealth = 10;
			
			_eventbus.Value.UniqueEvents.Add<EntitySpawnedEvent>();

			_performanceService.Value.EntityCount++;
			_lastSpawnTime = Time.time;
		}
	}
}