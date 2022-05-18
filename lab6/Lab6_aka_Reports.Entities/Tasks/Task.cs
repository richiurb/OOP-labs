using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab6_aka_Reports.Entities.Tasks
{
	public class Task : IdentifiedEntity
	{
		private readonly List<string> _comments;
		private readonly List<TaskHistoryItem> _historyItems;

		public Task(Employee employee, string title, string description, Employee assignee)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException("Title must be set", nameof(title));
			}

			CreationDate = TimeMachine.Now;

			_comments = new List<string>();
			_historyItems = new List<TaskHistoryItem>();

			SetTitle(employee, title);
			SetDescription(employee, description);
			SetAssignee(employee, assignee);
			SetState(employee, TaskState.Open);
		}

		public DateTime CreationDate { get; }

		public string Title { get; private set; }

		public string Description { get; private set; }

		public Employee Assignee { get; private set; }

		public TaskState? State { get; private set; }

		public string[] Comments => _comments.ToArray();

		public TaskHistoryItem[] HistoryItems => _historyItems.ToArray();

		public DateTime LastModificationDate => _historyItems.LastOrDefault()?.Date ?? CreationDate;

		public void SetTitle(Employee employee, string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				return;
			}

			var oldTitle = Title;
			Title = title;
			AddHistoryItem(employee, TaskHistoryItemType.Title, oldTitle, title);
		}

		public void SetDescription(Employee employee, string description)
		{
			if (Description == null && description == null)
			{
				return;
			}

			var oldDescription = Description;
			Description = description;
			AddHistoryItem(employee, TaskHistoryItemType.Description, oldDescription, description);
		}

		public void SetAssignee(Employee employee, Employee assignee)
		{
			if (Assignee?.Id == assignee?.Id)
			{
				return;
			}

			var oldAssignee = Assignee;
			Assignee = assignee;
			AddHistoryItem(employee, TaskHistoryItemType.Assignment, oldAssignee?.Name, assignee?.Name);
		}

		public void SetState(Employee employee, TaskState state)
		{
			if (State == state)
			{
				return;
			}

			var oldState = State;
			State = state;
			AddHistoryItem(employee, TaskHistoryItemType.State, oldState.ToString(), state.ToString());
		}

		public void AddComment(Employee employee, string content)
		{
			_comments.Add(content);
			AddHistoryItem(employee, TaskHistoryItemType.Comment, null, content);
		}

		private void AddHistoryItem(Employee employee, TaskHistoryItemType type, string oldValue, string newValue)
		{
			_historyItems.Add(new TaskHistoryItem(employee, type, TimeMachine.Now, oldValue, newValue));
		}
	}
}
