using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EventBusDemo.Helpers
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class MovingAverage
	{
		private readonly Queue<double> _internal_Items = new Queue<double>();
		public double Average { get; private set; }
		public uint SampleSize { get; }
		public int Precision { get; set; }
		public uint CurrentCount { get; private set; }
		private string DebuggerDisplay =>
			string.Format("Count: {0}/{1}, Average: {2}", CurrentCount, SampleSize, Average);


		public MovingAverage(uint count, int precision = 0)
		{
			if (count == 0)
				throw new ArgumentException("Unable to create moving average with 0 history entries.");

			SampleSize = count;
			Precision = precision;
		}


		public void Add(double value)
		{
			if (CurrentCount == SampleSize) RemoveOldest();
			AddNewItem(value);
		}


		private void AddNewItem(double value)
		{
			CurrentCount++;
			_internal_Items.Enqueue(value);
			Average = (value + (CurrentCount - 1) * Average) / CurrentCount;
			if (Precision != 0)
				Average = System.Math.Round(Average, Precision);
		}


		private void RemoveOldest()
		{
			var first = _internal_Items.Dequeue();
			if (CurrentCount > 1)
				Average = (Average * CurrentCount - first) / (CurrentCount - 1);
			else
				Average = 0;
			CurrentCount--;
			if (Precision != 0)
				Average = System.Math.Round(Average, Precision);
		}


		public void Clear()
		{
			Average = 0;
			CurrentCount = 0;
			_internal_Items.Clear();
		}
	}
}