using System;

namespace Lab6_aka_Reports.Entities.Reports
{
	public class SprintReport : Report
	{
		public SprintReport(Employee employee, DateTime fromDate, DateTime toDate) : base (employee)
		{
			FromDate = fromDate.Date;
			ToDate = toDate.Date;
		}

		public DateTime FromDate { get; }

		public DateTime ToDate { get; }

		public bool Locked { get; private set; }

		public void Lock()
		{
			Locked = true;
		}

		protected override bool ValidateDate(DateTime date) => !Locked && date >= FromDate && date < ToDate.AddDays(1);
	}
}
