using System;
using System.Collections.Generic;
using Backups_aka_Lab4.Cleaning;

namespace Backups_aka_Lab4
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("Enter storing policy name: ");
			var storingPolicy = CreateStoringPolicy(Console.ReadLine());

			var files = new List<BackupFile>();

			while (true)
			{
				files.Add(ReadNewFile());

				Console.WriteLine("Add one more file? y/n");
				var addMoreKey = Console.ReadKey(true).Key;

				if (addMoreKey == ConsoleKey.N)
				{
					break;
				}
			}

			var backup = new Backup(files, storingPolicy);

			while (true)
			{
				Console.Clear();

				Console.WriteLine(
					"1. Create restore point\n" +
					"2. Clean restore points\n" +
					"3. Add file\n" +
					"4. Remove file\n" +
					"5. Exit");

				var menuKey = Console.ReadKey(true).Key;

				Console.Clear();

				try
				{
					if (!HandleMenuAction(menuKey, backup))
					{
						break;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.ReadKey();
				}
			}
		}

		private static bool HandleMenuAction(ConsoleKey key, Backup backup)
		{
			switch (key)
			{
				case ConsoleKey.D1:
					CreateRestorePoint(backup);
					return true;

				case ConsoleKey.D2:
					CleanRestorePoints(backup);
					return true;

				case ConsoleKey.D3:
					AddFile(backup);
					return true;

				case ConsoleKey.D4:
					RemoveFile(backup);
					return true;

				case ConsoleKey.D5:
					return false;

				default:
					throw new ArgumentException("Invalid menu action");
			}
		}

		private static void CreateRestorePoint(Backup backup)
		{
			Console.Write("Enter creation policy name: ");
			var creationPolicy = CreateCreationPolicy(Console.ReadLine());

			var restorePoint = backup.CreateRestorePoint(creationPolicy);

			Console.WriteLine($"Restore point created! ID: {restorePoint.Id}, Size: {restorePoint.Size} bytes, Files:");

			foreach (var file in restorePoint.Files)
			{
				Console.WriteLine(file.FullPath);
			}

			Console.ReadKey();
		}

		private static void CleanRestorePoints(Backup backup)
		{
			var cleaningPolicy = AskForCleaningPolicy();
			var cleaningResult = backup.Clean(cleaningPolicy);

			Console.WriteLine($"Restore points cleaned! Backups count: {backup.RestorePoints.Count}");

			if (!cleaningResult)
			{
				Console.WriteLine("Not all restore points were deleted");
			}

			Console.ReadKey();
		}

		private static BackupFile ReadNewFile()
		{
			Console.Write("Enter file path: ");
			var path = Console.ReadLine();

			Console.Write("Enter file size: ");
			var size = long.Parse(Console.ReadLine());

			return new BackupFile(path, size);
		}

		private static void AddFile(Backup backup)
		{
			var file = ReadNewFile();
			backup.AddFile(file);

			Console.WriteLine("File added!");

			Console.ReadKey();
		}

		private static void RemoveFile(Backup backup)
		{
			Console.Write("Enter file path: ");
			var path = Console.ReadLine();

			if (backup.RemoveFile(path))
			{
				Console.WriteLine("File removed!");
			}
			else
			{
				Console.WriteLine("Unable to remove file");
			}

			Console.ReadKey();
		}

		private static ICleaningPolicy AskForCleaningPolicy()
		{
			Console.Write("Enter cleaning policy name: ");
			return CreateCleaningPolicy(Console.ReadLine());
		}

		private static IStoringPolicy CreateStoringPolicy(string name)
		{
			switch (name)
			{
				case "separate":
					return new SeparateStoringPolicy();
				case "archival":
					return new ArchivalStoringPolicy();
				default:
					throw new ArgumentException("Invalid storing policy name");
			}
		}

		private static ICreationPolicy CreateCreationPolicy(string name)
		{
			switch (name)
			{
				case "full":
					return new FullCreationPolicy();
				case "incremental":
					return new IncrementalCreationPolicy();
				default:
					throw new ArgumentException("Invalid creation policy name");
			}
		}

		private static ICleaningPolicy CreateCleaningPolicy(string name)
		{
			switch (name)
			{
				case "date":
					Console.Write("Enter minimum date: ");
					var minDate = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy HH:mm:ss", null);

					return new DateCleaningPolicy(minDate);
				case "quantity":
					Console.Write("Enter maximum quantity: ");
					var maxQuantity = int.Parse(Console.ReadLine());

					return new QuantityCleaningPolicy(maxQuantity);
				case "size":
					Console.Write("Enter maximum size: ");
					var maxSize = long.Parse(Console.ReadLine());

					return new SizeCleaningPolicy(maxSize);
				case "hybrid":
					var hybridPolicies = new List<ICleaningPolicy>();

					while (true)
					{
						hybridPolicies.Add(AskForCleaningPolicy());

						Console.WriteLine("Add one more policy? y/n");
						var addMoreKey = Console.ReadKey(true).Key;

						if (addMoreKey == ConsoleKey.N)
						{
							break;
						}
					}

					Console.Write("Enter polling method name: ");
					var pollingMethod = ParsePollingMethod(Console.ReadLine());

					return new HybridCleaningPolicy(hybridPolicies, pollingMethod);
				default:
					throw new ArgumentException("Invalid cleaning policy name");
			}
		}

		private static HybridCleaningPolicyPollingMethod ParsePollingMethod(string name)
		{
			switch (name)
			{
				case "all":
					return HybridCleaningPolicyPollingMethod.All;
				case "any":
					return HybridCleaningPolicyPollingMethod.Any;
				default:
					throw new ArgumentException("Invalid polling method name");
			}
		}
	}
}
