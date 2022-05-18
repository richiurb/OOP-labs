using System;
using System.Collections.Generic;
using System.Linq;
using Lab6_aka_Reports.Entities.Reports;

namespace Lab6_aka_Reports.DAL.InMemory
{
	public class DailyReportRepository : IDailyReportRepository
	{
		private readonly List<DailyReport> _reports;

		public DailyReportRepository()
		{
			_reports = new List<DailyReport>();
		}

		public void Insert(DailyReport report)
		{
			_reports.Add(report);
		}

		public DailyReport Get(Guid employeeId, DateTime date)
		{
			return _reports.FirstOrDefault(r => r.Employee.Id == employeeId && r.Date == date);
		}

		public void Update(DailyReport report)
		{
			return;
		}
	}
}
