using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Backups_aka_Lab4
{
	public class IncrementalCreationPolicy : ICreationPolicy
	{
		private const string IncrementalBackupsDirectory = @"C:\backups\incremental";

		public RestorePoint CreateRestorePoint(List<RestorePoint> restorePoints, List<BackupFile> files)
		{
			var restorePointId = Guid.NewGuid();
			var destinationDirectory = Path.Combine(IncrementalBackupsDirectory, restorePointId.ToString());

			var lastFullPointIndex = 0;

			for (int i = restorePoints.Count - 1; i >= 0; i--)
			{
				if (restorePoints[i].Type == RestorePointType.Full)
				{
					lastFullPointIndex = i;
				}
			}

			var lastRestorePoints = restorePoints.Skip(lastFullPointIndex + 1).ToList();
			var incrementFiles = GetIncrement(lastRestorePoints, files);

			foreach (var incrementFile in incrementFiles)
			{
				CopyFile(incrementFile, restorePointId, IncrementalBackupsDirectory);
			}

			return new RestorePoint(
				restorePointId,
				incrementFiles.ToArray(),
				DateTime.Now,
				RestorePointType.Incremental, 
				destinationDirectory);
		}

		private void CopyFile(BackupFile file, Guid id, string destinationDirectory)
		{
			// File.Copy(file.FullPath, Path.Combine(destinationDirectory, file.Name));
		}

		private List<BackupFile> GetIncrement(List<RestorePoint> lastRestorePoints, List<BackupFile> files)
		{
			// Evaluate and return increments for all files
			return files.Select(f => new BackupFile(f.FullPath + "_increment", f.Size)).ToList();
		}
	}
}
