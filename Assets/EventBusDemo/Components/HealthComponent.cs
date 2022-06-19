using Leopotam.EcsLite;

namespace EventBusDemo.Components
{
	public struct HealthComponent
	{
		public int MaxHealth;
		public int CurrentHealth;
	}

	public struct HealthLowFlag : IFlagComponent
	{
		
	}
}