using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Services
{
	public interface IViewService
	{
			// create a view from a premade asset (e.g. a prefab)
			void LoadPrefab(IServiceContainer services, EcsPackedEntityWithWorld entity, GameObject prefab);
			void LoadAsset(IServiceContainer services, EcsPackedEntityWithWorld entity, string assetName);
			void Destroy(EcsPackedEntityWithWorld packEntityWithWorld);
	}
}