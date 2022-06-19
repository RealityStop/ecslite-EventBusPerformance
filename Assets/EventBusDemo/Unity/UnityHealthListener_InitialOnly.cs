using EventBusDemo.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityHealthListener_InitialOnly : MonoBehaviour, IEventListener
	{
		private EcsPool<HealthComponent> _healthPool;
		private EcsPackedEntityWithWorld _entity;
		private SpriteRenderer _spriteRenderer;


		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}


		public void RegisterListeners(IServiceContainer container, EcsPackedEntityWithWorld packed)
		{
			_entity = packed;
			if (packed.Unpack(out var world, out var entityID))
			{
				_healthPool = world.GetPool<HealthComponent>();
				ref var health = ref _healthPool.Get(entityID);
				if (_spriteRenderer)
					_spriteRenderer.color = HealthColors.colorPresets[health.CurrentHealth];
			}
		}


		public void ReleaseListeners()
		{
		}
	}
	
	
	public static class HealthColors
	{
		public static Color[] colorPresets = new[]
		{
			new Color(255, 0, 0),
			new Color(255, 0, 0),
			new Color(255, 63, 0),
			new Color(255, 127, 0),
			new Color(255, 191, 0),
			new Color(255, 255, 0),
			new Color(204, 255, 0),
			new Color(153, 255, 0),
			new Color(102, 255, 0),
			new Color(51, 255, 0),
			new Color(0, 255, 0)
		};
	}
}