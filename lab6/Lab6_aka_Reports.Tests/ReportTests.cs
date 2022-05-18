using System;
using System.Linq;
using Lab6_aka_Reports.Entities;
using Lab6_aka_Reports.Entities.Reports;
using Lab6_aka_Reports.Entities.Tasks;
using NUnit.Framework;

namespace Lab6_aka_Reports.Tests
{
	[TestFixture]
	public class ReportTests : BaseTests
	{
		#region Append

		[Test]
		public void Should_append_items_to_daily_report()
		{
			var employee = GetDanglingEmployee();
			var report = new DailyReport(employee);

			var historyItem = new TaskHistoryItem(employee, TaskHistoryItemType.Title, TimeMachine.Now, GetRandomString(), GetRandomString());

			var resolvedTask = new Task(employee, GetRandomString(), GetRandomString(), employee);
			resolvedTask.SetState(employee, TaskState.Resolved);

			var result = report.Append(new[] { historyItem }, new[] { resolvedTask });

			Assert.IsTrue(result);
			Assert.AreEqual(1, report.HistoryItems.Length);
			Assert.AreEqual(1, report.ResolvedTasks.Length);
			Assert.AreEqual(historyItem, report.HistoryItems.First());
			Assert.AreEqual(resolvedTask, report.ResolvedTasks.First());
		}

		[Test]
		public void Should_append_items_to_sprint_report()
		{
			var employee = GetDanglingEmployee();
			var report = new SprintReport(employee, TimeMachine.Now, TimeMachine.Now.AddDays(1));

			var historyItem = new TaskHistoryItem(employee, TaskHistoryItemType.Title, TimeMachine.Now, GetRandomString(), GetRandomString());

			var resolvedTask = new Task(employee, GetRandomString(), GetRandomString(), employee);
			resolvedTask.SetState(employee, TaskState.Resolved);

			var result = report.Append(new[] { historyItem }, new[] { resolvedTask });

			Assert.IsTrue(result);
			Assert.AreEqual(1, report.HistoryItems.Length);
			Assert.AreEqual(1, report.ResolvedTasks.Length);
			Assert.AreEqual(historyItem, report.HistoryItems.First());
			Assert.AreEqual(resolvedTask, report.ResolvedTasks.First());
		}

		[Test]
		public void Should_not_append_items_to_daily_report_if_history_item_date_is_incorrect()
		{
			var employee = GetDanglingEmployee();
			var report = new DailyReport(employee);

			var historyItem = new TaskHistoryItem(employee, TaskHistoryItemType.Title, TimeMachine.Now.AddDays(-1), GetRandomString(), GetRandomString());

			var result = report.Append(new[] { historyItem }, Array.Empty<Task>());

			Assert.IsFalse(result);
			Assert.AreEqual(0, report.HistoryItems.Length);
		}

		[Test]
		public void Should_not_append_items_to_daily_report_if_resolved_task_date_is_incorrect()
		{
			var employee = GetDanglingEmployee();
			var report = new DailyReport(employee);

			var resolvedTask = new Task(employee, GetRandomString(), GetRandomString(), employee);

			TimeMachine.GoAhead(TimeSpan.FromDays(-1));
			resolvedTask.SetState(employee, TaskState.Resolved);

			var result = report.Append(Array.Empty<TaskHistoryItem>(), new[] { resolvedTask });

			Assert.IsFalse(result);
			Assert.AreEqual(0, report.ResolvedTasks.Length);
		}

		[Test]
		public void Should_not_append_items_to_daily_report_if_resolved_task_assignee_is_incorrect()
		{
			var employee1 = GetDanglingEmployee();
			var employee2 = GetDanglingEmployee();
			var report = new DailyReport(employee1);

			var resolvedTask = new Task(employee1, GetRandomString(), GetRandomString(), employee1);
			resolvedTask.SetAssignee(employee1, employee2);
			resolvedTask.SetState(employee2, TaskState.Resolved);

			var result = report.Append(Array.Empty<TaskHistoryItem>(), new[] { resolvedTask });

			Assert.IsFalse(result);
			Assert.AreEqual(0, report.ResolvedTasks.Length);
		}

		[Test]
		public void Should_not_append_items_to_sprint_report_if_history_item_date_is_incorrect()
		{
			var employee = GetDanglingEmployee();
			var report = new SprintReport(employee, TimeMachine.Now, TimeMachine.Now.AddDays(2));

			var historyItem = new TaskHistoryItem(employee, TaskHistoryItemType.Title, TimeMachine.Now.AddDays(-1), GetRandomString(), GetRandomString());

			var result = report.Append(new[] { historyItem }, Array.Empty<Task>());

			Assert.IsFalse(result);
			Assert.AreEqual(0, report.HistoryItems.Length);
		}

		[Test]
		public void Should_not_append_items_to_sprint_report_if_resolved_task_date_is_incorrect()
		{
			var employee = GetDanglingEmployee();
			var report = new SprintReport(employee, TimeMachine.Now, TimeMachine.Now.AddDays(2));

			var resolvedTask = new Task(employee, GetRandomString(), GetRandomString(), employee);

			TimeMachine.GoAhead(TimeSpan.FromDays(-1));
			resolvedTask.SetState(employee, TaskState.Resolved);

			var result = report.Append(Array.Empty<TaskHistoryItem>(), new[] { resolvedTask });

			Assert.IsFalse(result);
			Assert.AreEqual(0, report.ResolvedTasks.Length);
		}

		#endregion

		#region SprintReport

		[Test]
		public void Should_not_append_items_to_locked_sprint_report()
		{
			var employee = GetDanglingEmployee();
			var report = new SprintReport(employee, TimeMachine.Now, TimeMachine.Now.AddDays(1));
			report.Lock();

			var historyItem = new TaskHistoryItem(employee, TaskHistoryItemType.Title, TimeMachine.Now, GetRandomString(), GetRandomString());

			var result = report.Append(new[] { historyItem }, Array.Empty<Task>());

			Assert.IsFalse(result);
			Assert.AreEqual(0, report.HistoryItems.Length);
		}

		#endregion
	}
}
