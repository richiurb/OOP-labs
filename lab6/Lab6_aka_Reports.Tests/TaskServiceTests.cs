using System;
using System.Linq;
using Lab6_aka_Reports.BLL;
using Lab6_aka_Reports.DAL.InMemory;
using Lab6_aka_Reports.Entities;
using Lab6_aka_Reports.Entities.Tasks;
using NUnit.Framework;

namespace Lab6_aka_Reports.Tests
{
	[TestFixture]
	public class TaskServiceTests : BaseTests
	{
		private TaskService _taskService;

		[SetUp]
		public void Setup()
		{
			_taskService = new TaskService(new TaskRepository());
		}

		#region GetTaskById

		[Test]
		public void Should_get_task_by_id()
		{
			var task = GetTask();

			_taskService.Add(task);

			var result = _taskService.GetById(task.Id);

			Assert.IsNotNull(result);
			Assert.AreEqual(task, result);
		}

		[Test]
		public void Should_not_get_task_by_incorrect_id()
		{
			var task = GetTask();

			_taskService.Add(task);

			var result = _taskService.GetById(Guid.NewGuid());

			Assert.IsNull(result);
		}

		#endregion

		#region GetTasksByCreationDate

		[Test]
		public void Should_get_task_by_creation_date()
		{
			var task = GetTask();

			_taskService.Add(task);

			var result = _taskService.GetByCreationDate(TimeMachine.Now.AddHours(-1), TimeMachine.Now.AddHours(1));

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(task, result.First());
		}

		[Test]
		public void Should_not_get_task_by_incorrect_creation_date()
		{
			var task = GetTask();

			_taskService.Add(task);

			var result = _taskService.GetByCreationDate(TimeMachine.Now.AddHours(1), TimeMachine.Now.AddHours(2));

			Assert.AreEqual(0, result.Length);
		}

		#endregion

		#region GetTasksByLastModificationDate

		[Test]
		public void Should_get_task_by_last_modification_date()
		{
			var task = GetTask();
			task.SetTitle(task.Assignee, GetRandomString());

			_taskService.Add(task);

			var result = _taskService.GetByLastModificationDate(TimeMachine.Now.AddHours(-1), TimeMachine.Now.AddHours(1));

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(task, result.First());
		}

		[Test]
		public void Should_not_get_task_by_incorrect_last_modification_date()
		{
			var task = GetTask();
			task.SetTitle(task.Assignee, GetRandomString());

			_taskService.Add(task);

			var result = _taskService.GetByLastModificationDate(TimeMachine.Now.AddHours(1), TimeMachine.Now.AddHours(2));

			Assert.AreEqual(0, result.Length);
		}

		#endregion

		#region GetTasksByAssignee

		[Test]
		public void Should_get_task_by_assignee()
		{
			var task = GetTask();

			_taskService.Add(task);

			var result = _taskService.GetByAssignee(task.Assignee);

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(task, result.First());
		}

		[Test]
		public void Should_not_get_task_by_incorrect_assignee()
		{
			var task = GetTask();

			_taskService.Add(task);

			var result = _taskService.GetByAssignee(GetDanglingEmployee());

			Assert.AreEqual(0, result.Length);
		}

		#endregion

		#region GetTasksByEditor

		[Test]
		public void Should_get_task_by_editor()
		{
			var task = GetTask();
			var editor = GetDanglingEmployee();
			task.SetTitle(editor, GetRandomString());

			_taskService.Add(task);

			var result = _taskService.GetByEditor(editor);

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(task, result.First());
		}

		[Test]
		public void Should_not_get_task_by_incorrect_editor()
		{
			var task = GetTask();

			_taskService.Add(task);

			var result = _taskService.GetByEditor(GetDanglingEmployee());

			Assert.AreEqual(0, result.Length);
		}

		#endregion

		#region GetTasksByLeader

		[Test]
		public void Should_get_task_by_leader()
		{
			var task = GetTask();
			var leader = new Employee(GetRandomString(), null, new[] { task.Assignee });

			_taskService.Add(task);

			var result = _taskService.GetByLeader(leader);

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(task, result.First());
		}

		[Test]
		public void Should_not_get_task_by_incorrect_leader()
		{
			var task = GetTask();

			_taskService.Add(task);

			var result = _taskService.GetByLeader(GetDanglingEmployee());

			Assert.AreEqual(0, result.Length);
		}

		#endregion

		private Task GetTask()
		{
			var title = GetRandomString();
			var description = GetRandomString();

			var employee = GetDanglingEmployee();
			var assignee = GetDanglingEmployee();

			return new Task(employee, title, description, assignee);
		}
	}
}
