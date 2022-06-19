using EventBusDemo.Components;
using EventBusDemo.Events;
using Leopotam.EcsLite;

namespace EventBusDemo.Unity
{
	public class UnityPositionListener_EntityEvent : MonoEventListener
	{
		private EcsPool<PositionComponent> _positionPool;
		private IEventBus _eventBus;

		protected override void OnSubscribe()
		{
			_eventBus ??= Services.Get<IEventBus>();
			_eventBus.EntityEvents.SubscribeTo<PositionChanged>(PackedEntity, OnPositionChanged);
		}


		private void OnPositionChanged(EcsPackedEntityWithWorld packed, ref PositionChanged item)
		{
			transform.position = item.NewPosition;
		}


		protected override void OnUnsubscribe()
		{
			_eventBus.EntityEvents.RemoveListener<PositionChanged>(PackedEntity, OnPositionChanged);
		}
	}
}