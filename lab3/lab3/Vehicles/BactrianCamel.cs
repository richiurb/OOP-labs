namespace lab3.Vehicles
{
	class BactrianCamel : LandVehicle
	{
		public override double RestInterval => 30;

		public override double[] RestDuration => new double[] { 5, 8 };

		public override double Speed => 10;
	}
}
