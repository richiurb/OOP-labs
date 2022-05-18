using System;
using System.Collections.Generic;
using Lab6_aka_Reports.DAL;
using Lab6_aka_Reports.Entities;
using Lab6_aka_Reports.Entities.Reports;

namespace Lab6_aka_Reports.BLL
{
	public class ReportService : IReportService
	{
		private readonly IDailyReportRepository _dailyReportRepository;
		private readonly ISprintReportRepository _sprintReportRepository;

		public ReportService(
			IDailyReportRepository dailyReportRepository,
			ISprintReportRepository sprintReportRepository)
		{
			_dailyReportRepository = dailyReportRepository;
			_sprintReportRepository = sprintReportRepository;
		}

		public DailyReport CreateDailyReport(Employee employee)
		{
			var existingReport = _dailyReportRepository.Get(employee.Id, TimeMachine.Today);

			if (existingReport != null)
			{
				return existingReport;
			}

			var newReport = new DailyReport(employee);
			_dailyReportRepository.Insert(newReport);

			return newReport;
		}

		public SprintReport CreateSprintReport(Employee employee, DateTime fromDate, DateTime toDate)
		{
			var existingReport = _sprintReportRepository.Get(employee.Id, fromDate.Date, toDate.Date);

			if (existingReport != null)
			{
				return existingReport;
			}

			var newReport = new SprintReport(employee, fromDate, toDate);
			_sprintReportRepository.Insert(newReport);

			return newReport;
		}

		public SprintReport GetSprintReport(Employee employee, DateTime fromDate, DateTime toDate)
		{
			return _sprintReportRepository.Get(employee.Id, fromDate.Date, toDate.Date);
		}

		public DailyReport GetDailyReport(Employee employee, DateTime date)
		{
			return _dailyReportRepository.Get(employee.Id, date.Date);
		}

		public SprintReport[] GetSprintReportsByLeader(Employee employee, DateTime fromDate, DateTime toDate)
		{
			var subordinateReports = new List<SprintReport>();
			var ownReport = GetSprintReport(employee, fromDate, toDate);

			if (ownReport != null && ownReport.Locked)
			{
				subordinateReports.Add(ownReport);
			}

			var subordinates = employee.Subordinates;

			foreach (var subordinate in subordinates)
			{
				subordinateReports.AddRange(GetSprintReportsByLeader(subordinate, fromDate, toDate));
			}

			return subordinateReports.ToArray();
		}

		public void Update(SprintReport report)
		{
			_sprintReportRepository.Update(report);
		}

		public void Update(DailyReport report)
		{
			_dailyReportRepository.Update(report);
		}
	}
}
