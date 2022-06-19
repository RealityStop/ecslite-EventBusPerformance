using System;

namespace EventBusDemo.Services
{
	public interface ITimeService
	{
		float DeltaTime { get; }
		bool IsPaused { get; }
		Action<bool> PauseChanged { get; set; }
		void Pause();
	}
}