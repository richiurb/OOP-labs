namespace lab3.Vehicles
{
	class Mortar : AirVehicle
	{
		private const double ReductionPercent = 0.06;

		public override double Speed => 8;

		public override double ReduceDistance(double distance) => distance * (1 - ReductionPercent);
	}
}
