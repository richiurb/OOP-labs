using System;
using System.Collections.Generic;
using System.Linq;

namespace Backups_aka_Lab4.Cleaning
{
	public class SizeCleaningPolicy : ICleaningPolicy
	{
		private readonly long _maxSize;

		public SizeCleaningPolicy(long maxSize)
		{
			if (maxSize <= 0)
			{
				throw new ArgumentException("Max size should be greater than 0");
			}

			_maxSize = maxSize;
		}

		public (int fullsCountToDelete, bool allOk) Check(List<RestorePoint> restorePoints)
		{
			var totalSize = 0L;

			for (int i = restorePoints.Count - 1; i >= 0; i--)
			{
				var point = restorePoints[i];

				totalSize += point.Size;

				if (totalSize > _maxSize)
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
