using System;
using System.Linq;
using Lab6_aka_Reports.Entities;
using Lab6_aka_Reports.Entities.Tasks;
using NUnit.Framework;

namespace Lab6_aka_Reports.Tests
{
	[TestFixture]
	public class TaskTests : BaseTests
	{
		#region Constructor

		[Test]
		public void Should_not_create_task_with_no_title()
		{
			Assert.Throws<ArgumentException>(() => new Task(null, null, null, null));
		}

		[Test]
		public void Should_create_task_with_correct_data()
		{
			var title = GetRandomString();
			var description = GetRandomString();

			var employee = GetDanglingEmployee();
			var assignee = GetDanglingEmployee();

			var task = new Task(employee, title, description, assignee);

			Assert.AreEqual(title, task.Title);
			Assert.AreEqual(description, task.Description);
			Assert.AreEqual(assignee, task.Assignee);
			Assert.AreEqual(TaskState.Open, task.State);
			Assert.AreEqual(TimeMachine.Now, task.CreationDate);
		}

		#endregion

		#region SetTitle

		[Test]
		public void Should_set_title()
		{
			var title1 = GetRandomString();
			var title2 = GetRandomString();
			var description = GetRandomString();

			var employee = GetDanglingEmployee();
			var assignee = GetDanglingEmployee();

			var task = new Task(employee, title1, description, assignee);

			task.SetTitle(employee, title2);

			Assert.AreEqual(title2, task.Title);
		}

		[Test]
		public void Should_not_set_empty_title()
		{
			var title1 = GetRandomString();
			var title2 = string.Empty;
			var description = GetRandomString();

			var employee = GetDanglingEmployee();
			var assignee = GetDanglingEmployee();

			var task = new Task(employee, title1, description, assignee);

			task.SetTitle(employee, title2);

			Assert.AreEqual(title1, task.Title);
		}

		#endregion

		#region SetDescription

		[Test]
		public void Should_set_description()
		{
			var title = GetRandomString();
			var description1 = GetRandomString();
			var description2 = GetRandomString();

			var employee = GetDanglingEmployee();
			var assignee = GetDanglingEmployee();

			var task = new Task(employee, title, description1, assignee);

			task.SetDescription(employee, description2);

			Assert.AreEqual(description2, task.Description);
		}

		#endregion

		#region SetAssignee

		[Test]
		public void Should_set_assignee()
		{
			var title = GetRandomString();
			var description = GetRandomString();

			var employee = GetDanglingEmployee();
			var assignee1 = GetDanglingEmployee();
			var assignee2 = GetDanglingEmployee();

			var task = new Task(employee, title, description, assignee1);

			task.SetAssignee(employee, assignee2);

			Assert.AreEqual(assignee2, task.Assignee);
		}

		#endregion

		#region SetState

		[TestCase(TaskState.Open)]
		[TestCase(TaskState.Active)]
		[TestCase(TaskState.Resolved)]
		public void Should_set_state(TaskState state)
		{
			var title = GetRandomString();
			var description = GetRandomString();

			var employee = GetDanglingEmployee();
			var assignee = GetDanglingEmployee();

			var task = new Task(employee, title, description, assignee);

			task.SetState(employee, state);

			Assert.AreEqual(state, task.State);
		}

		#endregion

		#region AddComment

		[Test]
		public void Should_add_comment()
		{
			var title = GetRandomString();
			var description = GetRandomString();

			var employee = GetDanglingEmployee();
			var assignee = GetDanglingEmployee();

			var task = new Task(employee, title, description, assignee);

			var comment = GetRandomString();

			task.AddComment(employee, comment);

			Assert.AreEqual(1, task.Comments.Length);
			Assert.AreEqual(comment, task.Comments.First());
		}

		#endregion

		#region AddHistoryItem

		[Test]
		public void Should_add_history_items_on_change()
		{
			var title1 = GetRandomString();
			var title2 = GetRandomString();
			var description1 = GetRandomString();
			var description2 = GetRandomString();

			var employee = GetDanglingEmployee();
			var assignee1 = GetDanglingEmployee();
			var assignee2 = GetDanglingEmployee();

			var comment = GetRandomString();
			var state = TaskState.Active;

			var task = new Task(employee, title1, description1, assignee1);

			var oldHistory = task.HistoryItems;

			task.SetAssignee(employee, assignee2);
			task.SetTitle(employee, title2);
			task.SetDescription(employee, description2);
			task.SetState(employee, state);
			task.AddComment(employee, comment);

			var newHistory = task.HistoryItems;

			Assert.AreEqual(5, newHistory.Length - oldHistory.Length);

			Assert.IsNotNull(newHistory.SingleOrDefault(hi => 
				hi.Employee == employee && hi.Date == TimeMachine.Now && hi.Type == TaskHistoryItemType.Assignment && hi.OldValue == assignee1.Name && hi.NewValue == assignee2.Name));
			Assert.IsNotNull(newHistory.SingleOrDefault(hi =>
				hi.Employee == employee && hi.Date == TimeMachine.Now && hi.Type == TaskHistoryItemType.Title && hi.OldValue == title1 && hi.NewValue == title2));
			Assert.IsNotNull(newHistory.SingleOrDefault(hi =>
				hi.Employee == employee && hi.Date == TimeMachine.Now && hi.Type == TaskHistoryItemType.Description && hi.OldValue == description1 && hi.NewValue == description2));
			Assert.IsNotNull(newHistory.SingleOrDefault(hi =>
				hi.Employee == employee && hi.Date == TimeMachine.Now && hi.Type == TaskHistoryItemType.State && hi.OldValue == TaskState.Open.ToString() && hi.NewValue == state.ToString()));
			Assert.IsNotNull(newHistory.SingleOrDefault(hi =>
				hi.Employee == employee && hi.Date == TimeMachine.Now && hi.Type == TaskHistoryItemType.Comment && hi.OldValue == null && hi.NewValue == comment));

			Assert.AreEqual(TimeMachine.Now, task.LastModificationDate);
		}

		#endregion
	}
}
