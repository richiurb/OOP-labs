using System;
using System.Collections.Generic;
using System.Linq;
using Lab6_aka_Reports.BLL;
using Lab6_aka_Reports.Entities;
using Lab6_aka_Reports.Entities.Reports;
using Lab6_aka_Reports.Entities.Tasks;

namespace Lab6_aka_Reports.UI
{
	public class Program
	{
		private readonly IReportService _reportService;
		private readonly ITaskService _taskService;
		private readonly IEmployeeService _employeeService;

		public static void Main(string[] args)
		{
		}

		#region Employee

		public void CreateEmployee()
		{
			var name = Console.ReadLine();

			var leaderId = Guid.Parse(Console.ReadLine());
			var leader = _employeeService.Get(leaderId);

			var subordinates = new List<Employee>();

			while (true)
			{
				var subordinateIdText = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(subordinateIdText))
				{
					break;
				}

				var subordinateId = Guid.Parse(subordinateIdText);
				var subordinate = _employeeService.Get(subordinateId);

				subordinates.Add(subordinate);
			}

			_employeeService.Add(new Employee(name, leader, subordinates));
		}

		public void SetEmployeeLeader()
		{
			var employeeId = Guid.Parse(Console.ReadLine());
			var employee = _employeeService.Get(employeeId);

			var leaderId = Guid.Parse(Console.ReadLine());
			var leader = _employeeService.Get(leaderId);

			employee.SetLeader(leader);

			_employeeService.Update(employee);
		}

		#endregion

		#region Task

		public void CreateTask()
		{
			var employeeId = Guid.Parse(Console.ReadLine());
			var employee = _employeeService.Get(employeeId);

			var title = Console.ReadLine();
			var description = Console.ReadLine();

			var assigneeId = Guid.Parse(Console.ReadLine());
			var assignee = _employeeService.Get(assigneeId);

			var task = new Task(employee, title, description, assignee);

			_taskService.Add(task);
		}

		public void UpdateTask()
		{
			var employeeId = Guid.Parse(Console.ReadLine());
			var employee = _employeeService.Get(employeeId);

			var taskId = Guid.Parse(Console.ReadLine());
			var task = _taskService.GetById(taskId);

			var title = Console.ReadLine();
			var description = Console.ReadLine();

			var assigneeId = Guid.Parse(Console.ReadLine());
			var assignee = _employeeService.Get(assigneeId);

			task.SetTitle(employee, title);
			task.SetDescription(employee, description);
			task.SetAssignee(employee, assignee);

			_taskService.Update(task);
		}

		public void AddCommentToTask()
		{
			var employeeId = Guid.Parse(Console.ReadLine());
			var employee = _employeeService.Get(employeeId);

			var taskId = Guid.Parse(Console.ReadLine());
			var task = _taskService.GetById(taskId);

			var comment = Console.ReadLine();

			task.AddComment(employee, comment);

			_taskService.Update(task);
		}

		#endregion

		#region Report

		public DailyReport CreateDailyReport()
		{
			var employeeId = Guid.Parse(Console.ReadLine());
			var employee = _employeeService.Get(employeeId);

			return _reportService.CreateDailyReport(employee);
		}

		public SprintReport CreateSprintReport()
		{
			var employeeId = Guid.Parse(Console.ReadLine());
			var employee = _employeeService.Get(employeeId);

			var fromDate = DateTime.Parse(Console.ReadLine());
			var toDate = DateTime.Parse(Console.ReadLine());

			return _reportService.CreateSprintReport(employee, fromDate, toDate);
		}



		public void UpdateReport()
		{
			var employeeId = Guid.Parse(Console.ReadLine());
			var employee = _employeeService.Get(employeeId);

			var date = DateTime.Parse(Console.ReadLine());

			var reportId = Guid.Parse(Console.ReadLine());
			var report = _reportService.GetDailyReport(employee, date);

			var tasks = _taskService.GetByEditor(employee).ToArray();

			var historyItems = tasks
				.SelectMany(t => t.HistoryItems)
				.Where(hi => hi.Employee.Id == employeeId && hi.Date > report.LastHistoryDate)
				.ToArray();

			var resolvedTasks = tasks
				.Where(t => t.HistoryItems.Any(hi =>
					hi.Type == TaskHistoryItemType.State
					&& hi.Employee.Id == employeeId
					&& hi.Date > report.LastHistoryDate))
				.ToArray();

			report.Append(historyItems, resolvedTasks);

			_reportService.Update(report);
		}

		public SprintReport[] GetOverallSprintReport()
		{
			var employeeId = Guid.Parse(Console.ReadLine());
			var employee = _employeeService.Get(employeeId);

			var fromDate = DateTime.Parse(Console.ReadLine());
			var toDate = DateTime.Parse(Console.ReadLine());

			return _reportService.GetSprintReportsByLeader(employee, fromDate, toDate);
		}

		#endregion
	}
}
