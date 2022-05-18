using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Backups_aka_Lab4
{
	public class FullCreationPolicy : ICreationPolicy
	{
		private const string FullBackupsDirectory = @"C:\backups\full";

		public RestorePoint CreateRestorePoint(List<RestorePoint> restorePoints, List<BackupFile> files)
		{
			var restorePointId = Guid.NewGuid();
			var destinationDirectory = Path.Combine(FullBackupsDirectory, restorePointId.ToString());

			foreach (var file in files)
			{
				CopyFile(file, destinationDirectory);
			}

			return new RestorePoint(
				restorePointId, 
				files.ToArray(),
				DateTime.Now, 
				RestorePointType.Full,
				destinationDirectory);
		}

		private void CopyFile(BackupFile file, string destinationDirectory)
		{
			// File.Copy(file.FullPath, Path.Combine(destinationDirectory, file.Name));
		}
	}
}
