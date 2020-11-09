namespace lab3.Vehicles
{
	class Centaur : LandVehicle
	{
		public override double RestInterval => 8;

		public override double[] RestDuration => new double[] { 2 };

		public override double Speed => 15;
	}
}
