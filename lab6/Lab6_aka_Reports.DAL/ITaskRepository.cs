using System;
using System.Collections.Generic;
using Lab6_aka_Reports.Entities.Tasks;

namespace Lab6_aka_Reports.DAL
{
	public interface ITaskRepository
	{
		void Insert(Task task);

		Task Get(Guid id);

		Task[] GetByCreationDate(DateTime from, DateTime to);

		Task[] GetByLastModificationDate(DateTime from, DateTime to);

		Task[] GetByEditor(Guid employeeId);

		Task[] GetByAssignee(IEnumerable<Guid> employeeIds);

		void Update(Task task);
	}
}
