using System;

namespace lab3.Vehicles
{
	class Broom : AirVehicle
	{
		private const double ReductionThreshold = 1000;

		public override double Speed => 20;

		public override double ReduceDistance(double distance)
		{
			var reductionIntervals = Math.Floor(distance / ReductionThreshold);
			var reducedDistance = 0.0;

			for (int i = 0; i < reductionIntervals; i++)
			{
				reducedDistance += ReductionThreshold * (1 - (i + 1) * 0.01);
			}

			return reducedDistance + (distance - (reductionIntervals * ReductionThreshold)) * (1 - (reductionIntervals + 1) * 0.01);
		}
	}
}
