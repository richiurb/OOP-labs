using System;
using System.Collections.Generic;
using System.Linq;
using Lab6_aka_Reports.Entities.Tasks;

namespace Lab6_aka_Reports.DAL.InMemory
{
	public class TaskRepository : ITaskRepository
	{
		private readonly List<Task> _tasks;

		public TaskRepository()
		{
			_tasks = new List<Task>();
		}

		public void Insert(Task task)
		{
			_tasks.Add(task);
		}

		public Task Get(Guid id)
		{
			return _tasks.FirstOrDefault(t => t.Id == id);
		}

		public Task[] GetByCreationDate(DateTime from, DateTime to)
		{
			return _tasks
				.Where(t => t.CreationDate >= from && t.CreationDate <= to)
				.OrderBy(t => t.CreationDate)
				.ToArray();
		}

		public Task[] GetByLastModificationDate(DateTime from, DateTime to)
		{
			return _tasks
				.Where(t => t.LastModificationDate >= from && t.LastModificationDate <= to)
				.OrderBy(t => t.LastModificationDate)
				.ToArray();
		}

		public Task[] GetByEditor(Guid employeeId)
		{
			return _tasks
				.Where(t => t.HistoryItems.Any(i => i.Employee.Id == employeeId))
				.ToArray();
		}

		public Task[] GetByAssignee(IEnumerable<Guid> employeeIds)
		{
			return _tasks
				.Where(t => employeeIds.Contains(t.Assignee.Id))
				.ToArray();
		}

		public void Update(Task task)
		{
			return;
		}
	}
}
