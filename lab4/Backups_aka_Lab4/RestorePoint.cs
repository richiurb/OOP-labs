using System;
using System.Linq;

namespace Backups_aka_Lab4
{
	public enum RestorePointType
	{
		Full,
		Incremental
	}

	public class RestorePoint
	{
		public RestorePoint(Guid id, BackupFile[] files, DateTime creationTime, RestorePointType type, string directoryPath)
		{
			Id = id;
			CreationTime = creationTime;
			Files = files;
			Type = type;
			DirectoryPath = directoryPath;
			Size = files.Sum(f => f.Size);
		}

		public Guid Id { get; }

		public BackupFile[] Files { get; }

		public DateTime CreationTime { get; }

		public RestorePointType Type { get; }

		public string DirectoryPath { get; }

		public long Size { get; }
	}
}
