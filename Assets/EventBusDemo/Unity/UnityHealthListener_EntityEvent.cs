using EventBusDemo.Events;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityHealthListener_EntityEvent : MonoEventListener
	{
		private SpriteRenderer _spriteRenderer;
		private IEventBus _eventBus;


		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}


		protected override void OnSubscribe()
		{
			_eventBus = Services.Get<IEventBus>();
			_eventBus.EntityEvents.ListenTo<HealthChangedEvent>(PackedEntity, OnHealthChanged);
		}


		private void OnHealthChanged(EcsPackedEntityWithWorld packed, ref HealthChangedEvent item)
		{
			if (_spriteRenderer)
				_spriteRenderer.color =
					HealthColors.colorPresets[Mathf.Clamp((int)item.newHealth, 0, HealthColors.colorPresets.Length - 1)];
		}


		protected override void OnUnsubscribe()
		{
			base.OnUnsubscribe();
			_eventBus.EntityEvents.RemoveListener<HealthChangedEvent>(PackedEntity, OnHealthChanged);
		}
	}
}