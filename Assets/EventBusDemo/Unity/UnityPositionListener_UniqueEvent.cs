using EventBusDemo.Components;
using EventBusDemo.Events;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityPositionListener_UniqueEvent : MonoEventListener
	{
		private EcsPool<PositionComponent> _positionPool;
		private EcsPackedEntityWithWorld _entity;
		private Transform cachedTransform;
		private IEventBus _eventBus;


		protected override void OnSubscribe()
		{
			cachedTransform = transform;		//Caching the transform is about 1.5x faster than using the transform directly.
			_positionPool = UnsafeWorld.GetPool<PositionComponent>();
			_eventBus ??= Services.Get<IEventBus>();
			_eventBus.UniqueEvents.SubscribeTo<UpdateEvent>(OnUpdate);
		}


		private void OnUpdate(ref UpdateEvent item)
		{
			cachedTransform.position = _positionPool.Get(UnsafeEntity).Position;
		}


		protected override void OnUnsubscribe()
		{
			_eventBus.UniqueEvents.RemoveListener<UpdateEvent>(OnUpdate);
		}
	}
}