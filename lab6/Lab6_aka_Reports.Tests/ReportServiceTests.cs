using System;
using Lab6_aka_Reports.BLL;
using Lab6_aka_Reports.DAL.InMemory;
using Lab6_aka_Reports.Entities;
using Lab6_aka_Reports.Entities.Reports;
using NUnit.Framework;

namespace Lab6_aka_Reports.Tests
{
	[TestFixture]
	public class ReportServiceTests : BaseTests
	{
		private ReportService _reportService;

		[SetUp]
		public void Setup()
		{
			_reportService = new ReportService(
				new DailyReportRepository(),
				new SprintReportRepository());
		}

		#region CreateReport

		[Test]
		public void Should_create_only_one_sprint_report_for_dates()
		{
			var employee = GetDanglingEmployee();

			var report1 = _reportService.CreateSprintReport(employee, TimeMachine.Now, TimeMachine.Now.AddDays(1));
			var report2 = _reportService.CreateSprintReport(employee, TimeMachine.Now, TimeMachine.Now.AddDays(1));

			Assert.AreEqual(report1, report2);
		}

		[Test]
		public void Should_create_only_one_daily_report_for_one_date()
		{
			var employee = GetDanglingEmployee();

			var report1 = _reportService.CreateDailyReport(employee);
			var report2 = _reportService.CreateDailyReport(employee);

			Assert.AreEqual(report1, report2);
			Assert.AreEqual(TimeMachine.Today, report1.Date);
		}

		[Test]
		public void Should_create_several_daily_reports_for_several_days()
		{
			var employee = GetDanglingEmployee();

			var time1 = TimeMachine.Today;
			var report1 = _reportService.CreateDailyReport(employee);

			TimeMachine.GoAhead(TimeSpan.FromDays(1));
			var time2 = TimeMachine.Today;
			var report2 = _reportService.CreateDailyReport(employee);

			Assert.AreNotEqual(report1, report2);
			Assert.AreEqual(time1, report1.Date);
			Assert.AreEqual(time2, report2.Date);
		}

		#endregion

		#region GetSprintReportsByLeader

		[Test]
		public void Should_create_aggregate_sprint_report_for_leader()
		{
			var leader = GetDanglingEmployee();
			var employeeLvl11 = GetDanglingEmployee();
			var employeeLvl12 = GetDanglingEmployee();
			var employeeLvl21 = GetDanglingEmployee();
			var employeeLvl22 = GetDanglingEmployee();
			var employeeLvl23 = GetDanglingEmployee();

			leader.AddSubordinate(employeeLvl11);
			leader.AddSubordinate(employeeLvl12);

			employeeLvl11.AddSubordinate(employeeLvl21);
			employeeLvl11.AddSubordinate(employeeLvl22);
			employeeLvl11.AddSubordinate(employeeLvl23);

			var reportLeader = AddDefaultSprintReport(leader);
			var reportEmployeeLvl11 = AddDefaultSprintReport(employeeLvl11);
			var reportEmployeeLvl12 = AddDefaultSprintReport(employeeLvl12);
			var reportEmployeeLvl21 = AddDefaultSprintReport(employeeLvl21);
			var reportEmployeeLvl22 = AddDefaultSprintReport(employeeLvl22);
			var reportEmployeeLvl23 = _reportService.CreateSprintReport(employeeLvl23, TimeMachine.Now.AddDays(-3), TimeMachine.Now.AddDays(-2));

			reportLeader.Lock();
			reportEmployeeLvl12.Lock();
			reportEmployeeLvl22.Lock();
			reportEmployeeLvl23.Lock();

			var reports = _reportService.GetSprintReportsByLeader(leader, reportLeader.FromDate, reportLeader.ToDate);

			Assert.AreEqual(3, reports.Length);
			Assert.Contains(reportLeader, reports);
			Assert.Contains(reportEmployeeLvl12, reports);
			Assert.Contains(reportEmployeeLvl22, reports);
		}

		private SprintReport AddDefaultSprintReport(Employee employee)
		{
			return _reportService.CreateSprintReport(employee, TimeMachine.Now.AddDays(-1), TimeMachine.Now.AddDays(1));
		}

		#endregion
	}
}
