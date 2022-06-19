using EventBusDemo.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace EventBusDemo.Unity
{
	public class UnityDisplayService : MonoBehaviour, IDisplayService, IHostedService
	{
		[SerializeField] private Camera TargetCamera;


		private void Awake()
		{
			TopLeft = TargetCamera.ScreenToWorldPoint(Vector3.zero);
			TopRight = TargetCamera.ScreenToWorldPoint(new Vector3(TargetCamera.pixelWidth, 0, 0));
			BottomLeft = TargetCamera.ScreenToWorldPoint(new Vector3(0, TargetCamera.pixelHeight, 0));
			BottomRight =
				TargetCamera.ScreenToWorldPoint(new Vector3(TargetCamera.pixelWidth, TargetCamera.pixelHeight, 0));

			Top = TopLeft.y;
			Left = TopLeft.x;
			Right = BottomRight.x;
			Bottom = BottomRight.y;
		}


		public int Width => TargetCamera.pixelWidth;
		public int Height => TargetCamera.pixelHeight;

		public float Top { get; private set; }
		public float Left { get; private set; }
		public float Right { get; private set; }
		public float Bottom { get; private set; }


		public Vector2 TopLeft { get; private set; }

		public Vector2 TopRight { get; private set; }
		public Vector2 BottomLeft { get; private set; }

		public Vector2 BottomRight { get; private set; }
	}
}