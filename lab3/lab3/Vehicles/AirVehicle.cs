namespace lab3.Vehicles
{
	abstract class AirVehicle : VehicleBase
	{
		public abstract double ReduceDistance(double distance);

		public override double CalculateTime(double distance)
		{
			var realDistance = ReduceDistance(distance);
			return realDistance / Speed;
		}
	}
}
