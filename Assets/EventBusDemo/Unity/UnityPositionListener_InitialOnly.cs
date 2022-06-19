using EventBusDemo.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityPositionListener_InitialOnly : MonoBehaviour, IEventListener
	{
		private EcsPool<PositionComponent> _positionPool;
		private EcsPackedEntityWithWorld _entity;

		public void RegisterListeners(IServiceContainer container, EcsPackedEntityWithWorld packed)
		{
			_entity = packed;
			if (packed.Unpack(out var world, out var entityID))
			{
				_positionPool = world.GetPool<PositionComponent>();
				transform.position = _positionPool.Get(entityID).Position;
			}
		}
		
		public void ReleaseListeners()
		{
		}
	}
}