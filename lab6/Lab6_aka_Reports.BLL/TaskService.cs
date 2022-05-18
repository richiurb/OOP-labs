using System;
using System.Linq;
using Lab6_aka_Reports.DAL;
using Lab6_aka_Reports.Entities;
using Lab6_aka_Reports.Entities.Tasks;

namespace Lab6_aka_Reports.BLL
{
	public class TaskService : ITaskService
	{
		private readonly ITaskRepository _taskRepository;

		public TaskService(ITaskRepository taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public void Add(Task task)
		{
			if (task == null)
			{
				return;
			}

			_taskRepository.Insert(task);
		}

		public Task GetById(Guid id)
		{
			return _taskRepository.Get(id);
		}

		public Task[] GetByCreationDate(DateTime from, DateTime to)
		{
			return _taskRepository.GetByCreationDate(from, to);
		}

		public Task[] GetByLastModificationDate(DateTime from, DateTime to)
		{
			return _taskRepository.GetByLastModificationDate(from, to);
		}

		public Task[] GetByAssignee(Employee employee)
		{
			return _taskRepository.GetByAssignee(new[] { employee.Id });
		}

		public Task[] GetByEditor(Employee employee)
		{
			return _taskRepository.GetByEditor(employee.Id);
		}

		public Task[] GetByLeader(Employee employee)
		{
			var subordinateIds = employee.Subordinates.Select(s => s.Id).ToArray();

			return _taskRepository.GetByAssignee(subordinateIds);
		}

		public void Update(Task task)
		{
			_taskRepository.Update(task);
		}
	}
}
