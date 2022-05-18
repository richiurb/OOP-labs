using System.Collections.Generic;
using System.Linq;

namespace Backups_aka_Lab4.Cleaning
{
	public enum HybridCleaningPolicyPollingMethod
	{
		All,
		Any
	}

	public class HybridCleaningPolicy : ICleaningPolicy
	{
		private readonly List<ICleaningPolicy> _policies;
		private readonly HybridCleaningPolicyPollingMethod _pollingMethod;

		public HybridCleaningPolicy(List<ICleaningPolicy> policies, HybridCleaningPolicyPollingMethod pollingMethod)
		{
			_policies = policies;
			_pollingMethod = pollingMethod;
		}

		public (int fullsCountToDelete, bool allOk) Check(List<RestorePoint> restorePoints)
		{
			var results = _policies.Select(policy => policy.Check(restorePoints)).ToList();

			if (_pollingMethod == HybridCleaningPolicyPollingMethod.All)
			{
				var minimum = results.Min(result => result.fullsCountToDelete);
				return (minimum, results.Any(result => result.fullsCountToDelete == minimum && result.allOk));
			}
			
			var maximum = results.Max(result => result.fullsCountToDelete);	
			return (maximum, !results.Any(result => result.fullsCountToDelete == maximum && !result.allOk));
		}
	}
}
