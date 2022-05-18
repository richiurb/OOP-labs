using System;

namespace Lab6_aka_Reports.Entities.Reports
{
	public class DailyReport : Report
	{
		public DailyReport(Employee employee) : base (employee)
		{
			Date = TimeMachine.Today;
		}

		public DateTime Date { get; }

		protected override bool ValidateDate(DateTime date) => date >= Date && date < Date.AddDays(1);
	}
}
