namespace lab3.Vehicles
{
	abstract class VehicleBase
	{
		public abstract double Speed { get; }

		public abstract double CalculateTime(double distance);
	}
}
