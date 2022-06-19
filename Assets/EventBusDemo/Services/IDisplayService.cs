using UnityEngine;

namespace EventBusDemo.Services
{
	public interface IDisplayService
	{
		int Width { get; }
		int Height { get; }

		float Top { get; }
		Vector2 TopLeft { get; }
		float Left { get; }
		Vector2 TopRight { get; }
		float Right { get; }
		Vector2 BottomLeft { get; }
		float Bottom { get; }
		Vector2 BottomRight { get; }
	}
}