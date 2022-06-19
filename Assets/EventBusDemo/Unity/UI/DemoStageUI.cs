using EventBusDemo.Events;
using EventBusDemo.Services;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

namespace EventBusDemo.Unity.UI
{
	public class DemoStageUI : MonoBehaviour
	{
		private IDemo _demoService;
		private TMP_Text _label;


		private void Awake()
		{
			_label = GetComponentInChildren<TMP_Text>();
		}


		private void Start()
		{
			_demoService = ServiceContainer.Get<IDemo>();
			ServiceContainer.Get<IEventBus>().UniqueEvents.SubscribeTo<DemoStageChanged>(OnDemoChange);

			DemoStageChanged changed;
			OnDemoChange(ref changed);
		}


		private void OnDemoChange(ref DemoStageChanged item)
		{
			_label.text = $"Stage: {_demoService.CurrentDemoSettings.Phase}";
		}
	}
}