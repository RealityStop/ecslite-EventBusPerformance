using EventBusDemo.Services;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EventBusDemo.Unity.UI
{
	public class ResultsPanel : MonoBehaviour
	{
		[SerializeField] private TMP_Text resultsEntityDisplay;
		[SerializeField] private TMP_Text resultsMemoryDisplay;

		private bool _activated;
		private IDemo _demo;
		private Image _image;


		private void Awake()
		{
			_image = GetComponent<Image>();
			ActivateChildren(false);
		}


		private void Start()
		{
			_demo = ServiceContainer.Get<IDemo>();
		}


		private void Update()
		{
			if (!_activated && _demo.CurrentDemoSettings.IsComplete)
			{
				ActivateChildren(true);
				_activated = true;
				
				//resultsEntityDisplay.text = _demo.FinalEntitiesSpawned.ToString();
				//resultsMemoryDisplay.text = _demo.MemoryUsed.ToString();
			}
		}


		private void ActivateChildren(bool active)
		{
			_image.enabled = active;
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(active);				
			}
		}
	}
}