using System;
using Lab6_aka_Reports.Entities;
using Lab6_aka_Reports.Entities.Reports;

namespace Lab6_aka_Reports.BLL
{
	public interface IReportService
	{
		DailyReport CreateDailyReport(Employee employee);

		SprintReport CreateSprintReport(Employee employee, DateTime fromDate, DateTime toDate);

		SprintReport GetSprintReport(Employee employee, DateTime fromDate, DateTime toDate);

		DailyReport GetDailyReport(Employee employee, DateTime date);

		SprintReport[] GetSprintReportsByLeader(Employee employee, DateTime fromDate, DateTime toDate);

		void Update(SprintReport report);

		void Update(DailyReport report);
	}
}
