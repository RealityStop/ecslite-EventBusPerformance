using EventBusDemo.Services;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

namespace EventBusDemo.Unity.UI
{
	public class EntityPanel : MonoBehaviour
	{
		
		[SerializeField] private TMP_Text EntityDisplay;
		private IPerformanceService _performanceService;


		private void Start()
		{
			_performanceService = ServiceContainer.Get<IPerformanceService>();
		}


		private void Update()
		{
			EntityDisplay.text = $"Entities : {_performanceService.EntityCount}";
	
		}
	}
}