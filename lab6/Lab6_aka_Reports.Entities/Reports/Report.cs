using System;
using System.Collections.Generic;
using System.Linq;
using Lab6_aka_Reports.Entities.Tasks;

namespace Lab6_aka_Reports.Entities.Reports
{
	public abstract class Report
	{
		private readonly List<TaskHistoryItem> _historyItems;
		private readonly List<Task> _resolvedTasks;

		public Report(Employee employee)
		{
			Employee = employee;

			_historyItems = new List<TaskHistoryItem>();
			_resolvedTasks = new List<Task>();
		}

		protected abstract bool ValidateDate(DateTime date);

		public Employee Employee { get; }

		public DateTime? LastHistoryDate => _historyItems.LastOrDefault()?.Date;

		public TaskHistoryItem[] HistoryItems => _historyItems.ToArray();

		public Task[] ResolvedTasks => _resolvedTasks.ToArray();

		public bool Append(IEnumerable<TaskHistoryItem> historyItems, IEnumerable<Task> resolvedTasks)
		{
			if (!ValidateDate(TimeMachine.Now))
			{
				return false;
			}

			var historyItemsAreValid = historyItems.All(hi => hi.Employee.Id == Employee.Id && ValidateDate(hi.Date));

			var resolvedTasksAreValid = resolvedTasks.All(t =>
			{
				if (t.State != TaskState.Resolved || t.Assignee.Id != Employee.Id)
				{
					return false;
				}

				var lastStateModification = t.HistoryItems.LastOrDefault(i => i.Type == TaskHistoryItemType.State);
				return ValidateDate(lastStateModification.Date);
			});

			if (!historyItemsAreValid || !resolvedTasksAreValid)
			{
				return false;
			}

			_historyItems.AddRange(historyItems);
			_resolvedTasks.AddRange(resolvedTasks);

			return true;
		}
	}
}
