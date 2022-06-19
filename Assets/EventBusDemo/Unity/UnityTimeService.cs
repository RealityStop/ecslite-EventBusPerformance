using System;
using EventBusDemo.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityTimeService : MonoBehaviour, ITimeService, IHostedService
	{
		private void Update()
		{
			DeltaTime = Time.deltaTime;
		}


		public float DeltaTime { get; private set; }
		public bool IsPaused { get; private set; }
		public Action<bool> PauseChanged { get; set; }

		public void Pause()
		{
			Time.timeScale = 0;
			IsPaused = true;
			PauseChanged?.Invoke(IsPaused);
		}
	}
}