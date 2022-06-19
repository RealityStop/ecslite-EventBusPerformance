using System;
using EventBusDemo.Components;
using EventBusDemo.Events;
using Leopotam.EcsLite;

namespace EventBusDemo.Unity
{
	public class UnityPositionListener_FlagComponent : MonoEventListener
	{
		private IDisposable _subscription;
		private EcsPool<PositionComponent> _positionPool;
		private IEventBus _eventBus;


		protected override void OnSubscribe()
		{
			_positionPool = UnsafeWorld.GetPool<PositionComponent>();
			_eventBus ??= Services.Get<IEventBus>();
			_eventBus.FlagComponents.ListenTo<PositionChangedComponent>(PackedEntity, OnPositionChanged);
		}


		private void OnPositionChanged(EcsPackedEntityWithWorld obj)
		{
			transform.position = _positionPool.Get(UnsafeEntity).Position;
		}


		protected override void OnUnsubscribe()
		{
			base.OnUnsubscribe();
			_eventBus.FlagComponents.RemoveListener<PositionChangedComponent>(PackedEntity, OnPositionChanged);
		}
	}
}