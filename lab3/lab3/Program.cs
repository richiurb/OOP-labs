using System;
using System.Collections.Generic;
using lab3.Racings;
using lab3.Vehicles;

namespace lab3
{
	class Program
	{
		private static readonly Dictionary<string, Func<VehicleBase>> _vehicleFactories = new Dictionary<string, Func<VehicleBase>>
		{
			{ "bactriancamel", () => new BactrianCamel() },
			{ "speedcamel", () => new SpeedCamel() },
			{ "centaur", () => new Centaur() },
			{ "allterrainboots", () => new AllTerrainBoots() },
			{ "magiccarpet", () => new MagicCarpet() },
			{ "mortar", () => new Mortar() },
			{ "broom", () => new Broom() }
		};

		private static readonly Dictionary<string, Func<double, RaceBase>> _raceFactories = new Dictionary<string, Func<double, RaceBase>>
		{
			{ "land", d => new LandRace(d) },
			{ "air", d => new AirRace(d) },
			{ "all", d => new AllTypesRace(d) }
		};

		static void Main(string[] args)
		{
			var race = AskForRace();

			while (true)
			{
				var vehicle = AskForNewVehicle();

				if (race.TryAddVehicle(vehicle))
				{
					ColorPrint(ConsoleColor.Green, "Vehicle registered successfully");
				}
				else
				{
					ColorPrint(ConsoleColor.Red, "Unable to register vehicle");
				}

				Console.WriteLine("Add one more vehicle? y/n");
				var addMoreResult = Console.ReadKey(true);

				if (addMoreResult.Key == ConsoleKey.N)
				{
					break;
				}
			}

			var result = race.Start();

			ColorPrint(ConsoleColor.Yellow, $"Winner is {result.vehicle.GetType().Name}, time: {result.time}");
			Console.ReadKey();
		}

		static RaceBase AskForRace()
		{
			Console.Write("Enter race type: ");
			var raceType = Console.ReadLine().ToLower();

			if (!_raceFactories.TryGetValue(raceType, out var factory))
			{
				ColorPrint(ConsoleColor.Red, "Race type is invalid");
				return AskForRace();
			}

			var distance = AskForDistance();
			return factory(distance);
		}

		static VehicleBase AskForNewVehicle()
		{
			Console.Write("Enter vehicle name: ");
			var vehicleName = Console.ReadLine().ToLower();

			if (!_vehicleFactories.TryGetValue(vehicleName, out var factory))
			{
				ColorPrint(ConsoleColor.Red, "Vehicle name is invalid");
				return AskForNewVehicle();
			}

			return factory();
		}

		static double AskForDistance()
		{
			Console.Write("Enter distance: ");

			if (!double.TryParse(Console.ReadLine(), out var distance) || distance < 0)
			{
				ColorPrint(ConsoleColor.Red, "Distance must be positive");
				return AskForDistance();
			}

			return distance;
		}

		static void ColorPrint(ConsoleColor color, string text)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(text);
			Console.ResetColor();
		}
	}
}
