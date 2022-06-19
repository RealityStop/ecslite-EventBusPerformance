using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EventBusDemo.Events;
using EventBusDemo.Services;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace EventBusDemo.Unity
{
	internal struct DemoResults
	{
		public int FinalEntityCount;
	}


	public class UnityDemoController : MonoBehaviour, IDemo, IHostedService
	{
		[SerializeField] private GameObject _staticPrefab;
		[SerializeField] private int _maxEntityCount;

		public int MaxEntityCount => _maxEntityCount;

		[FormerlySerializedAs("SpawnThreshold")] public float LowFPSTimeThreshold;
		public float SwitchTimeGate;
		private IEventBus _eventBus;


		private bool _hasBeenAboveMinimum;
		private float _mostRecentlyAboveMinimumTime;
		private float _mostRecentSwitchedTime;
		private IPerformanceService _performanceService;


		private ITimeService _timeService;

		private Dictionary<DemoPhase, DemoResults> Results { get; } = new();


		private void Start()
		{
			_timeService = ServiceContainer.Get<ITimeService>();
			_performanceService = ServiceContainer.Get<IPerformanceService>();
			_eventBus = ServiceContainer.Get<IEventBus>();

			AdvancePhase();
		}


		private void Update()
		{
			if (CurrentDemoSettings.IsComplete)
				return;


			if (!_performanceService.LowFPSWarning)
			{
				_hasBeenAboveMinimum = true;
				_mostRecentlyAboveMinimumTime = Time.time;
			}


			if (!_hasBeenAboveMinimum)
				return;

			if (Time.time < _mostRecentSwitchedTime + SwitchTimeGate)
				return;

			bool timeTrigger = Time.time > _mostRecentlyAboveMinimumTime + LowFPSTimeThreshold;
			bool maxEntityTrigger = _performanceService.EntityCount >= _maxEntityCount;
			
			if (timeTrigger || maxEntityTrigger)
			{
				RecordResults();
				AdvancePhase();
			}
		}


		public DemoSettings CurrentDemoSettings { get; } = new();


		private void RecordResults()
		{
			Results[CurrentDemoSettings.Phase] = new DemoResults
			{
				FinalEntityCount = _performanceService.EntityCount
			};
		}


		private void AdvancePhase()
		{
			//Advance which phase is registered before any conditions can fire.
			CurrentDemoSettings.Phase = ++CurrentDemoSettings.Phase;

			//Clean up the trackers
			_performanceService.Reset();
			_eventBus.UniqueEvents.Add<DestroyAllEntitiesEvent>();
			// we also reset the timers so we have a minimum time to cover any overhead
			_mostRecentlyAboveMinimumTime = Time.time;
			_mostRecentSwitchedTime = Time.time;

			switch (CurrentDemoSettings.Phase)
			{
				case DemoPhase.MaxEntities:
					CurrentDemoSettings.ViewPrefab = null;
					CurrentDemoSettings.SpawnRateMultiplier = 10;
					CurrentDemoSettings.PositionChanges = DemoEventStyle.None;
					CurrentDemoSettings.HealthChanges = DemoEventStyle.None;
					break;
				case DemoPhase.MaxGameObjectViews:
					CurrentDemoSettings.EnableViews = true;
					CurrentDemoSettings.SpawnRateMultiplier = 1;
					CurrentDemoSettings.ViewPrefab = _staticPrefab;
					CurrentDemoSettings.PositionChanges = DemoEventStyle.InitialOnly;
					CurrentDemoSettings.HealthChanges = DemoEventStyle.None;
					break;
				case DemoPhase.PollingPosition:
					CurrentDemoSettings.EnableViews = true;
					CurrentDemoSettings.SpawnRateMultiplier = 1;
					CurrentDemoSettings.ViewPrefab = _staticPrefab;
					CurrentDemoSettings.PositionChanges = DemoEventStyle.Polling;
					CurrentDemoSettings.HealthChanges = DemoEventStyle.None;
					break;
				case DemoPhase.OptimizedUpdates:
					CurrentDemoSettings.EnableViews = true;
					CurrentDemoSettings.SpawnRateMultiplier = 1;
					CurrentDemoSettings.ViewPrefab = _staticPrefab;
					CurrentDemoSettings.PositionChanges = DemoEventStyle.UniqueEvent;
					CurrentDemoSettings.HealthChanges = DemoEventStyle.None;
					break;
				case DemoPhase.PollingHealth:
					CurrentDemoSettings.EnableViews = true;
					CurrentDemoSettings.SpawnRateMultiplier = 1;
					CurrentDemoSettings.ViewPrefab = _staticPrefab;
					CurrentDemoSettings.PositionChanges = DemoEventStyle.UniqueEvent;
					CurrentDemoSettings.HealthChanges = DemoEventStyle.Polling;
					break;
				case DemoPhase.SporadicHealth:
					CurrentDemoSettings.EnableViews = true;
					CurrentDemoSettings.SpawnRateMultiplier = 1;
					CurrentDemoSettings.ViewPrefab = _staticPrefab;
					CurrentDemoSettings.PositionChanges = DemoEventStyle.UniqueEvent;
					CurrentDemoSettings.HealthChanges = DemoEventStyle.Event;
					break;
				case DemoPhase.SporadicHealthFlagComponents:
					CurrentDemoSettings.EnableViews = true;
					CurrentDemoSettings.SpawnRateMultiplier = 1;
					CurrentDemoSettings.ViewPrefab = _staticPrefab;
					CurrentDemoSettings.PositionChanges = DemoEventStyle.UniqueEvent;
					CurrentDemoSettings.HealthChanges = DemoEventStyle.FlagComponents;
					break;
				case DemoPhase.Complete:
					using (var writer = new StreamWriter("performance.log"))
					{
						var phase = DemoPhase.MaxEntities;
						while (phase != DemoPhase.Complete)
						{
							if (Results.TryGetValue(phase, out var phaseResults))
							{
								writer.WriteLine($";;{phase.ToString()}");
								writer.WriteLine($"{phaseResults.FinalEntityCount}");
							}

							phase++;
						}
					}

					Process.Start("performance.log");

#if UNITY_EDITOR
					EditorApplication.ExitPlaymode();
#endif

					Application.Quit();
					break;
			}

			_eventBus.UniqueEvents.Add<DemoStageChanged>();
		}
	}
}