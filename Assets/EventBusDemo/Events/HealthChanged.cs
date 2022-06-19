using Leopotam.EcsLite;

namespace EventBusDemo.Events
{
	public struct HealthChangedEvent : IEventEntity
	{
		//Entity Events reference a specific entity, and we need a place to store that.  Since C#
		//interfaces don't allow fields, we have to use a property here.
		public EcsPackedEntityWithWorld Source { get; set; }
		
		//But regular data can be fields.  
		public float newHealth;
		public float newHealthPercent;
	}
	
	public struct HealthChangedFlag : IFlagComponent
	{
	}
}