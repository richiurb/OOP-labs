using lab3.Vehicles;

namespace lab3.Racings
{
	class LandRace : RaceBase
	{
		public LandRace(double distance) : base(distance)
		{
		}

		protected override bool CheckVehicle(VehicleBase vehicle)
		{
			return vehicle is LandVehicle;
		}
	}
}
