namespace lab3.Vehicles
{
	class AllTerrainBoots : LandVehicle
	{
		public override double RestInterval => 60;

		public override double[] RestDuration => new double[] { 10, 5 };

		public override double Speed => 6;
	}
}
