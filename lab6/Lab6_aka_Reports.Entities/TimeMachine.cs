using System;

namespace Lab6_aka_Reports.Entities
{
	public static class TimeMachine
	{
		static TimeMachine()
		{
			Now = DateTime.Now;
		}

		public static DateTime Now { get; private set; }

		public static DateTime Today => Now.Date;

		public static void GoAhead(TimeSpan timeSpan)
		{
			if (timeSpan.Seconds < 0)
			{
				throw new ArgumentException("Time span must be positive");
			}

			Now = Now.Add(timeSpan);
		}
	}
}
