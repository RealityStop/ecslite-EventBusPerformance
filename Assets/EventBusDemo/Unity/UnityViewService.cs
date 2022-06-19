using System.Collections.Generic;
using EventBusDemo.Helpers.Pooling;
using EventBusDemo.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityViewService : MonoBehaviour, IViewService, IHostedService
	{
		private Dictionary<string, GameObject> assetToPrefab = new Dictionary<string, GameObject>();
		private Dictionary<EcsPackedEntityWithWorld, IViewController> entityToViewController = new Dictionary<EcsPackedEntityWithWorld, IViewController>();


		private void Awake()
		{
			PoolManager.Instance.root = new GameObject("Pools").transform;
		}


		public void LoadAsset(IServiceContainer services, EcsPackedEntityWithWorld entity, string assetName)
		{
			var prefab = FetchPrefab(assetName);
			
			LoadPrefab(services, entity, prefab);
		}

		
		public void LoadPrefab(IServiceContainer services, EcsPackedEntityWithWorld entity, GameObject prefab)
		{
			var viewGo = PoolManager.SpawnObject(prefab, Vector3.zero, Quaternion.identity);
			if (viewGo != null)
			{
				var viewController = viewGo.GetComponent<IViewController>();
				if (viewController != null)
				{
					viewController.InitializeView(services, entity);
					entityToViewController.Add(entity, viewController);
				}
			}
		}
		

		public void Destroy(EcsPackedEntityWithWorld packEntityWithWorld)
		{
			if (entityToViewController.TryGetValue(packEntityWithWorld, out var viewController))
			{
				viewController.DestroyView();
				entityToViewController.Remove(packEntityWithWorld);
			}
		}


		private GameObject FetchPrefab(string assetName)
		{
			GameObject prefab;
			if (!assetToPrefab.TryGetValue(assetName, out prefab))
			{
				prefab = Resources.Load<GameObject>("Prefabs/" + assetName);
				assetToPrefab.Add(assetName, prefab);
				PoolManager.WarmPool(prefab, 5000);
			}

			return prefab;
		}
	}
}