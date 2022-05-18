using System;
using System.Collections.Generic;
using Lab6_aka_Reports.Entities.Tasks;

namespace Lab6_aka_Reports.DAL.Database
{
	public class TaskRepository : ITaskRepository
	{
		public Task Get(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task[] GetByAssignee(IEnumerable<Guid> employeeIds)
		{
			throw new NotImplementedException();
		}

		public Task[] GetByCreationDate(DateTime from, DateTime to)
		{
			throw new NotImplementedException();
		}

		public Task[] GetByEditor(Guid employeeId)
		{
			throw new NotImplementedException();
		}

		public Task[] GetByLastModificationDate(DateTime from, DateTime to)
		{
			throw new NotImplementedException();
		}

		public void Insert(Task task)
		{
			throw new NotImplementedException();
		}

		public void Update(Task task)
		{
			throw new NotImplementedException();
		}
	}
}
