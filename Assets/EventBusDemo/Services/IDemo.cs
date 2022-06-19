using UnityEngine;

namespace EventBusDemo.Services
{
	public enum DemoEventStyle
	{
		None,
		InitialOnly,
		Polling,
		Event,
		UniqueEvent,
		FlagComponents
	}
	
	
	public enum DemoPhase { 
		Startup,	
		
		//First, we need to count how many entities we can have with the demo scene systems.
		MaxEntities,
			
		//Then we need to test how many we can have if a game object is used to represent each entity. 
		MaxGameObjectViews,
			
		//1 Event Per Entity
		PollingPosition,
			
		//using a unique event to minimize the cost of polling position.
		OptimizedUpdates,

			
		//And now highlight the difference between polling
		PollingHealth,
			
		//And using relevant events in a "realistic" scenario
		SporadicHealth,
		
		//And using relevant events in a "realistic" scenario
		SporadicHealthFlagComponents,
			
		Complete }
	
	public class DemoSettings
	{
		public DemoPhase Phase { get; set; } = DemoPhase.Startup;

		public float SpawnRateMultiplier { get; set; } = 1;
		
		public bool EnableViews { get; set; }
		public GameObject ViewPrefab { get; set; }
		public DemoEventStyle PositionChanges { get; set; }
		public DemoEventStyle HealthChanges { get; set; }
		
		
		public bool IsComplete => Phase == DemoPhase.Complete;
	}
	

	public interface IDemo
	{
		int MaxEntityCount { get; }
		DemoSettings CurrentDemoSettings { get; }
	}
}