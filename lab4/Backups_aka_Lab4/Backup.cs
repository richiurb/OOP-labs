using System.Collections.Generic;
using System.Linq;

namespace Backups_aka_Lab4
{
	public class Backup
	{
		private readonly List<BackupFile> _files;

		private readonly IStoringPolicy _storingPolicy;

		public Backup(IEnumerable<BackupFile> files, IStoringPolicy storingPolicy)
		{
			_files = files.ToList();
			_storingPolicy = storingPolicy;

			RestorePoints = new List<RestorePoint>();
		}

		public List<RestorePoint> RestorePoints { get; }

		public long Size => RestorePoints.Sum(point => point.Size);

		public void AddFile(BackupFile file)
		{
			_files.Add(file);
		}

		public bool RemoveFile(string filePath)
		{
			var file = _files.FirstOrDefault(f => f.FullPath == filePath);

			if (file == null)
			{
				return false;
			}

			_files.Remove(file);
			return true;
		}

		public RestorePoint CreateRestorePoint(ICreationPolicy creationPolicy)
		{
			var preparedFiles = _storingPolicy.Prepare(_files);
			var restorePoint = creationPolicy.CreateRestorePoint(RestorePoints, preparedFiles);

			RestorePoints.Add(restorePoint);

			return restorePoint;
		}

		public bool Clean(ICleaningPolicy cleaningPolicy)
		{
			var (fullsCountToDelete, allOk) = cleaningPolicy.Check(RestorePoints);

			while (true)
			{
				var point = RestorePoints.First();

				if (point.Type == RestorePointType.Full)
				{
					if (fullsCountToDelete == 0)
					{
						break;
					}

					fullsCountToDelete--;
				}

				// Directory.Delete(point.DirectoryPath);
				RestorePoints.Remove(point);
			}

			return allOk;
		}
	}
}
