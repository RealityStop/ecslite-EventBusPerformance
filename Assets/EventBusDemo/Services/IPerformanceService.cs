namespace EventBusDemo.Services
{
	public interface IPerformanceService
	{
		public float AverageFPS { get; }
		public bool LowFPSWarning { get; }

		public int EntityCount { get; set; }
		long TotalMemoryUsed { get; }
		bool HighFPSNotification { get; }
		bool StopSpawning { get; }
		void Reset();
	}
}