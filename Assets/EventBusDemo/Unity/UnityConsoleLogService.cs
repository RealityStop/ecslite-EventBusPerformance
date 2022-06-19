using EventBusDemo.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityConsoleLogService : MonoBehaviour, ILogService, IHostedService
	{
		public void LogMessage(string message)
		{
			Debug.Log(message);
		}
	}
}