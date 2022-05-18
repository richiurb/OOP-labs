using System.IO;

namespace Backups_aka_Lab4
{
	public class BackupFile
	{
		public BackupFile(string filePath, long size)
		{
			FullPath = filePath;
			Name = Path.GetFileName(filePath);
			Size = size;
		}

		public string FullPath { get; }

		public string Name { get; }

		public long Size { get; set; }
	}
}
