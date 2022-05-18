using System.Collections.Generic;

namespace Backups_aka_Lab4
{
	public class SeparateStoringPolicy : IStoringPolicy
	{
		public List<BackupFile> Prepare(List<BackupFile> files)
		{
			return files;
		}
	}
}
