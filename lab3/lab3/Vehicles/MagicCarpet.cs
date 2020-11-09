namespace lab3.Vehicles
{
	class MagicCarpet : AirVehicle
	{
		private (double distanceThreshold, double percent)[] _distanceReduction = new []
		{
			( 1000, 0 ),
			( 5000, 0.03 ),
			( 10000, 0.1 ),
			( double.MaxValue, 0.05 )
		};

		public override double Speed => 10;

		public override double ReduceDistance(double distance)
		{
			var reducedDistance = 0.0;

			for (int i = 0; i < _distanceReduction.Length; i++)
			{
				var lastReductionDistanceThreshold = i == 0 ? 0.0 : _distanceReduction[i - 1].distanceThreshold;
				var (currentDistanceThreshold, currentPercent) = _distanceReduction[i];

				if (distance <= currentDistanceThreshold)
				{
					reducedDistance += (distance - lastReductionDistanceThreshold) * (1 - currentPercent);
					return reducedDistance;
				}

				reducedDistance += (currentDistanceThreshold - lastReductionDistanceThreshold) * (1 - currentPercent);
			}

			return 0;
		}
	}
}
