using System;
using Lab6_aka_Reports.Entities.Reports;

namespace Lab6_aka_Reports.DAL
{
	public interface IDailyReportRepository
	{
		void Insert(DailyReport report);

		DailyReport Get(Guid employeeId, DateTime date);

		void Update(DailyReport report);
	}
}
