using System.Collections.Generic;

namespace Backups_aka_Lab4
{
	public interface IStoringPolicy
	{
		List<BackupFile> Prepare(List<BackupFile> files);
	}
}
