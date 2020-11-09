using System;
using System.Linq;

namespace lab3.Vehicles
{
	abstract class LandVehicle : VehicleBase
	{
		public abstract double RestInterval { get; }

		public abstract double[] RestDuration { get; }

		public override double CalculateTime(double distance)
		{ 
			var time = distance / Speed;
			return time + GetOverallRestDuration(time);
		}

		private double GetOverallRestDuration(double time)
		{
			var rests = Math.Floor(time / RestInterval);
			var overallDuration = 0.0;

			for (int i = 0; i < rests; i++)
			{
				overallDuration += GetRestDuration(i);
			}

			return overallDuration;
		}

		private double GetRestDuration(int index) => index < RestDuration.Length ? RestDuration[index] : RestDuration.Last();
	}
}
