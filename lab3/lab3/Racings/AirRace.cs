using lab3.Vehicles;

namespace lab3.Racings
{
	class AirRace : RaceBase
	{
		public AirRace(double distance) : base(distance)
		{
		}

		protected override bool CheckVehicle(VehicleBase vehicle)
		{
			return vehicle is AirVehicle;
		}
	}
}
