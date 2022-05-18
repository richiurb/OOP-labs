using System.Collections.Generic;

namespace Backups_aka_Lab4
{
	public interface ICleaningPolicy
	{
		(int fullsCountToDelete, bool allOk) Check(List<RestorePoint> restorePoints);
	}
}
