using System.Collections.Generic;
using EventBusDemo.Components;
using EventBusDemo.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EventBusDemo.Systems
{
	public class LoadAssetSystem :IEcsRunSystem
	{
		private readonly EcsServiceInject<IViewService> _viewService;
		private readonly EcsServiceInject<IDemo> _demoService;

		private readonly EcsSharedInject<IServiceContainer> _services;

		private readonly EcsPoolInject<ViewComponent> _viewPool;
		private readonly EcsPoolInject<AssetComponent> _assetPool;


		private Dictionary<EcsPackedEntityWithWorld, IViewController> _entityViewMapping =
			new Dictionary<EcsPackedEntityWithWorld, IViewController>();

		private readonly EcsFilterInject<Inc<AssetComponent>, Exc<ViewComponent>> _missingViewsFromAsset;
		private readonly EcsFilterInject<Inc<DemoPrefabComponent>, Exc<ViewComponent>> _missingViewsFromPrefab;

		public void Run(EcsSystems systems)
		{
			foreach (var missingViewEntity in _missingViewsFromAsset.Value)
			{
				ref var asset = ref _missingViewsFromAsset.Pools.Inc1.Get(missingViewEntity);

				if (_demoService.Value.CurrentDemoSettings.EnableViews)
					_viewService.Value.LoadAsset(_services.Value,  systems.GetWorld().PackEntityWithWorld(missingViewEntity), asset.Name);
				//Lastly, add the asset component, so it won't be caught in future passes.
				ref var newView = ref _viewPool.Value.Add(missingViewEntity);
			}
			
			
			foreach (var missingViewEntity in _missingViewsFromPrefab.Value)
			{
				ref var asset = ref _missingViewsFromPrefab.Pools.Inc1.Get(missingViewEntity);

				if (_demoService.Value.CurrentDemoSettings.EnableViews)
					_viewService.Value.LoadPrefab(_services.Value,  systems.GetWorld().PackEntityWithWorld(missingViewEntity), _demoService.Value.CurrentDemoSettings.ViewPrefab);
				//Lastly, add the asset component, so it won't be caught in future passes.
				ref var newView = ref _viewPool.Value.Add(missingViewEntity);
			}
		}
	}
}