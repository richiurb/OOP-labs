using lab3.Vehicles;

namespace lab3.Racings
{
	class AllTypesRace : RaceBase
	{
		public AllTypesRace(double distance) : base(distance)
		{
		}

		protected override bool CheckVehicle(VehicleBase vehicle)
		{
			return true;
		}
	}
}
