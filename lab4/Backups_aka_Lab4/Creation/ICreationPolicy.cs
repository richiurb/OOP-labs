using System.Collections.Generic;

namespace Backups_aka_Lab4
{
	public interface ICreationPolicy
	{
		RestorePoint CreateRestorePoint(List<RestorePoint> restorePoints, List<BackupFile> files);
	}
}
