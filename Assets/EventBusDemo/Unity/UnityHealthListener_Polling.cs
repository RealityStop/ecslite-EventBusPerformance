using EventBusDemo.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityHealthListener_Polling : MonoBehaviour, IEventListener
	{
		private EcsPool<HealthComponent> _healthPool;
		private EcsPackedEntityWithWorld _entity;
		private SpriteRenderer _spriteRenderer;
		
		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}
		
		public void RegisterListeners(IServiceContainer container, EcsPackedEntityWithWorld pack)
		{
			_entity = pack;
			if (_entity.Unpack(out var world, out var entity))
				_healthPool = world.GetPool<HealthComponent>();
		}



		private void Update()
		{
			if (_entity.Unpack(out var world, out var entity))
			{
				ref var health = ref _healthPool.Get(entity);
				if (_spriteRenderer)
					_spriteRenderer.color = HealthColors.colorPresets[Mathf.Clamp(health.CurrentHealth, 0, HealthColors.colorPresets.Length-1)];
			}
		}


		public void ReleaseListeners()
		{
			
		}
	}
}