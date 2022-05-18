using System.Collections.Generic;
using System.Linq;

namespace Backups_aka_Lab4
{
	public class ArchivalStoringPolicy : IStoringPolicy
	{
		public List<BackupFile> Prepare(List<BackupFile> files)
		{
			return new List<BackupFile>(new[]
			{
				new BackupFile("archive", files.Sum(f => f.Size))
			});
		}
	}
}
