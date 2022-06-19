using System;
using EventBusDemo.Components;
using EventBusDemo.Events;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityHealthListener_FlagComponents : MonoEventListener
	{
		private EcsPool<HealthComponent> _healthPool;
		private SpriteRenderer _spriteRenderer;
		private IDisposable _subscription;
		private IEventBus _eventBus;


		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}
		
		protected override void OnSubscribe()
		{
			_healthPool = UnsafeWorld.GetPool<HealthComponent>();
			_eventBus ??= Services.Get<IEventBus>();
			_eventBus.FlagComponents.ListenTo<HealthChangedFlag>(PackedEntity, OnHealthChanged);
		}


		private void OnHealthChanged(EcsPackedEntityWithWorld obj)
		{
				ref var health = ref _healthPool.Get(UnsafeEntity);
				if (_spriteRenderer)
					_spriteRenderer.color = HealthColors.colorPresets[Mathf.Clamp(health.CurrentHealth, 0, HealthColors.colorPresets.Length-1)];
		}


		protected override void OnUnsubscribe()
		{
			_eventBus.FlagComponents.RemoveListener<HealthChangedFlag>(PackedEntity, OnHealthChanged);
		}
	}
}