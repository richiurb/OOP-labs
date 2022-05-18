using System;
using Lab6_aka_Reports.Entities;

namespace Lab6_aka_Reports.BLL
{
	public interface IEmployeeService
	{
		void Add(Employee employee);

		Employee Get(Guid id);

		void Update(Employee employee);
	}
}
