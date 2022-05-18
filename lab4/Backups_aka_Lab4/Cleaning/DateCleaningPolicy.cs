using System;
using System.Collections.Generic;
using System.Linq;

namespace Backups_aka_Lab4.Cleaning
{
	public class DateCleaningPolicy : ICleaningPolicy
	{
		private readonly DateTime _minDate;

		public DateCleaningPolicy(DateTime minDate)
		{
			_minDate = minDate;
		}

		public (int fullsCountToDelete, bool allOk) Check(List<RestorePoint> restorePoints)
		{
			for (int i = restorePoints.Count - 1; i >= 0; i--)
			{
				var point = restorePoints[i];

				if (point.CreationTime < _minDate)
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
