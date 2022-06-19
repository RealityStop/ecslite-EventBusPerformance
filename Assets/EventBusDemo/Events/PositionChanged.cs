using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Events
{
	public struct PositionChanged : IEventEntity
	{
		public EcsPackedEntityWithWorld Source { get; set; }

		public Vector2 NewPosition;
	}
}