using System;
using System.Collections.Generic;
using System.Linq;

namespace Backups_aka_Lab4.Cleaning
{
	public class QuantityCleaningPolicy : ICleaningPolicy
	{
		private readonly int _maxQuantity;

		public QuantityCleaningPolicy(int maxQuantity)
		{
			if (maxQuantity <= 0)
			{
				throw new ArgumentException("Max quantity should be greater than 0");
			}

			_maxQuantity = maxQuantity;
		}

		public (int fullsCountToDelete, bool allOk) Check(List<RestorePoint> restorePoints)
		{
			var quantity = 0;

			for (int i = restorePoints.Count - 1; i >= 0; i--)
			{
				var point = restorePoints[i];

				if (++quantity > _maxQuantity)
				{
					var allOk = point.Type == RestorePointType.Full
						|| i + 1 == restorePoints.Count
						|| restorePoints[i + 1].Type == RestorePointType.Full;

					var fullsCountToDelete = restorePoints
						.Take(i + 1)
						.Count(point => point.Type == RestorePointType.Full);

					return (allOk ? fullsCountToDelete : fullsCountToDelete - 1, allOk);
				}
			}

			return (0, true);
		}
	}
}
