using System;
using Lab6_aka_Reports.Entities;
using Lab6_aka_Reports.Entities.Tasks;

namespace Lab6_aka_Reports.BLL
{
	public interface ITaskService
	{
		void Add(Task task);

		Task GetById(Guid id);

		Task[] GetByCreationDate(DateTime from, DateTime to);

		Task[] GetByLastModificationDate(DateTime from, DateTime to);

		Task[] GetByAssignee(Employee employee);

		Task[] GetByEditor(Employee employee);

		Task[] GetByLeader(Employee employee);

		void Update(Task task);
	}
}
