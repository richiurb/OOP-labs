using System;

namespace Lab6_aka_Reports.Entities.Tasks
{
	public class TaskHistoryItem : IdentifiedEntity
	{
		public TaskHistoryItem(Employee employee, TaskHistoryItemType type, DateTime date, string oldValue, string newValue)
		{
			Employee = employee;
			Type = type;
			Date = date;

			OldValue = oldValue;
			NewValue = newValue;
		}

		public Employee Employee { get; }

		public DateTime Date { get; }

		public TaskHistoryItemType Type { get; }

		public string OldValue { get; }

		public string NewValue { get; }
	}
}
