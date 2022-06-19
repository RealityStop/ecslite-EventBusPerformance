using EventBusDemo.Helpers.Pooling;
using EventBusDemo.Services;
using EventBusDemo.Unity;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EventBusDemo.Views
{
	public class UnityViewControllerBase : ViewController
	{

		[SerializeField]private bool Pooled;
		
		[SerializeField] private UnityPositionListener_Polling _pollingPosition;
		[SerializeField] private UnityPositionListener_UniqueEvent _optimizedPosition;
		[SerializeField] private UnityHealthListener_Polling _pollingHealth;
		[SerializeField] private UnityHealthListener_EntityEvent _eventHealth;
		[SerializeField] private UnityHealthListener_FlagComponents _flagHealth;


		
		private void Awake()
		{
			_pollingPosition.enabled = false;
			_optimizedPosition.enabled = false;
			_pollingHealth.enabled = false;
			_eventHealth.enabled = false;
			_flagHealth.enabled = false;
		}


		protected override void OnPreInitializeListeners(IServiceContainer serviceContainer)
		{
			var demo = serviceContainer.Get<IDemo>();

			if (demo.CurrentDemoSettings.PositionChanges == DemoEventStyle.Polling)
				_pollingPosition.enabled = true;
			else if (demo.CurrentDemoSettings.PositionChanges == DemoEventStyle.UniqueEvent)
				_optimizedPosition.enabled = true;

			if (demo.CurrentDemoSettings.HealthChanges == DemoEventStyle.Polling)
				_pollingHealth.enabled = true;
			else if (demo.CurrentDemoSettings.HealthChanges == DemoEventStyle.Event)
				_eventHealth.enabled = true;
			else if (demo.CurrentDemoSettings.HealthChanges == DemoEventStyle.FlagComponents)
				_flagHealth.enabled = true;
		}


		protected override void OnPreDestroyListeners()
		{
			_pollingPosition.enabled = false;
			_optimizedPosition.enabled = false;
			_pollingHealth.enabled = false;
			_eventHealth.enabled = false;
			_flagHealth.enabled = false;
		}


		public override void OnViewDestroyed()
		{
			if (this is IPooled || Pooled)
				PoolManager.ReleaseObject(gameObject);
			else
				Object.Destroy(this);
		}
	}
}