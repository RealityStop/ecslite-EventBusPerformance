using EventBusDemo.Services;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

namespace EventBusDemo.Unity.UI
{
    public class FPSPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text FPSDisplay;
    
    
        private IPerformanceService _performanceService;


        private void Start()
        {
            _performanceService = ServiceContainer.Get<IPerformanceService>();
        }
    
    

        private void Update()
        {
            FPSDisplay.text = $"FPS : {_performanceService.AverageFPS.ToString("0.00")}";
        }
    }
}
