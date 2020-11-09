namespace lab3.Vehicles
{
	class SpeedCamel : LandVehicle
	{
		public override double RestInterval => 10;

		public override double[] RestDuration => new double[] { 5, 6.5, 8 };

		public override double Speed => 40;
	}
}
