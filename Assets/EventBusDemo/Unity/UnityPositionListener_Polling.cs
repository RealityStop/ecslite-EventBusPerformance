using EventBusDemo.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityPositionListener_Polling : MonoBehaviour, IEventListener
	{
		private EcsPool<PositionComponent> _positionPool;
		private EcsPackedEntityWithWorld _entity;
		public void RegisterListeners(IServiceContainer container, EcsPackedEntityWithWorld pack)
		{
			_entity = pack;
			if (_entity.Unpack(out var world, out var entity))
				_positionPool = world.GetPool<PositionComponent>();
		}
		
		private void Update()
		{
			if (_entity.Unpack(out var world, out var entity))
				transform.position = _positionPool.Get(entity).Position;
		}
		
		public void ReleaseListeners()
		{
		}
	}
}