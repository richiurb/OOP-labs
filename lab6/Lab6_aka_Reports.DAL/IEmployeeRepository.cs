using System;
using Lab6_aka_Reports.Entities;

namespace Lab6_aka_Reports.DAL
{
	public interface IEmployeeRepository
	{
		Employee Get(Guid id);

		void Insert(Employee employee);

		void Update(Employee employee);
	}
}
