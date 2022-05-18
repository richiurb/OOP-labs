using System;
using System.Collections.Generic;

namespace Lab5_aka_Banks
{
	public static class TimeMachine
	{
		public static event EventHandler<DateTime> OnDayChanged;

		static TimeMachine()
		{
			Now = DateTime.Now;
		}

		public static DateTime Now { get; private set; }

		public static void GoAhead(int days)
		{
			if (days < 0)
			{
				throw new ArgumentException("Days count must be positive");
			}

			for (int i = 0; i < days; i++)
			{
				Now = Now.AddDays(1);
				OnDayChanged.Invoke(null, Now);
			}
		}
	}
}
