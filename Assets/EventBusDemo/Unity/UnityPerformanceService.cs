using EventBusDemo.Helpers;
using EventBusDemo.Services;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Profiling;

namespace EventBusDemo.Unity
{
	public class UnityPerformanceService : MonoBehaviour, IPerformanceService, IHostedService
	{
		[SerializeField] private float warningFPS = 60;
		[SerializeField] private float minimumFPS = 59;
		[SerializeField] private float highFPSMarker = 85;
		private MovingAverage _movingAverage = new MovingAverage(200, 2);
		
		public float AverageFPS { get; private set; }
		public bool HighFPSNotification { get; private set; }
		public bool LowFPSWarning { get; private set; }
		
		public bool StopSpawning { get; private set; }
		
		public int EntityCount { get; set; }
		public int EventCount { get; set; }
		public long TotalMemoryUsed => Profiler.GetMonoHeapSizeLong();
		


		private void Update()
		{
			if (Time.timeScale != 0)
			{
				_movingAverage.Add(1 / Time.deltaTime);
				AverageFPS = (float)_movingAverage.Average;
				LowFPSWarning = AverageFPS < warningFPS;
				StopSpawning = AverageFPS < minimumFPS;
				HighFPSNotification = AverageFPS > highFPSMarker;
			}
		}
		
		public void Reset()
		{
			_movingAverage.Clear();
			LowFPSWarning = false;
			HighFPSNotification = false;
			AverageFPS = 0;
			EntityCount = 0;
		}
	}
}