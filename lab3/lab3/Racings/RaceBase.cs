using System.Collections.Generic;
using System.Linq;
using lab3.Vehicles;

namespace lab3.Racings
{
	abstract class RaceBase
	{
		private readonly List<VehicleBase> _vehicles;
		private readonly double _distance;

		public RaceBase(double distance)
		{
			_distance = distance;
			_vehicles = new List<VehicleBase>();
		}

		protected abstract bool CheckVehicle(VehicleBase vehicle);

		public bool TryAddVehicle(VehicleBase vehicle)
		{
			if (!CheckVehicle(vehicle))
			{
				return false;
			}

			_vehicles.Add(vehicle);
			return true;
		}

		public (VehicleBase vehicle, double time) Start()
		{
			if (!_vehicles.Any())
			{
				return (null, 0.0);
			}

			var firstVehicle = _vehicles.First();
			(VehicleBase vehicle, double time) winner = (firstVehicle, firstVehicle.CalculateTime(_distance));

			for (int i = 1; i < _vehicles.Count; i++)
			{
				var vehicle = _vehicles[i];
				var time = vehicle.CalculateTime(_distance);

				if (_vehicles[i].CalculateTime(_distance) < winner.time)
				{
					winner = (vehicle, time);
				}
			}

			return winner;
		}
	}
}
