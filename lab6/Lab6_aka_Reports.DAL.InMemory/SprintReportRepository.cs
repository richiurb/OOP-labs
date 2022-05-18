using System;
using System.Collections.Generic;
using System.Linq;
using Lab6_aka_Reports.Entities.Reports;

namespace Lab6_aka_Reports.DAL.InMemory
{
	public class SprintReportRepository : ISprintReportRepository
	{
		private readonly List<SprintReport> _reports;

		public SprintReportRepository()
		{
			_reports = new List<SprintReport>();
		}

		public void Insert(SprintReport report)
		{
			_reports.Add(report);
		}

		public SprintReport Get(Guid employeeId, DateTime fromDate, DateTime toDate)
		{
			return _reports.FirstOrDefault(r => r.Employee.Id == employeeId && r.FromDate == fromDate && r.ToDate == toDate);
		}

		public void Update(SprintReport report)
		{
			return;
		}
	}
}
