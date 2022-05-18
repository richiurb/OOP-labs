using System;
using Lab6_aka_Reports.DAL;
using Lab6_aka_Reports.Entities;

namespace Lab6_aka_Reports.BLL
{
	public class EmployeeService
	{
		private readonly IEmployeeRepository _employeeRepository;

		public EmployeeService(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		public void Add(Employee employee)
		{
			_employeeRepository.Insert(employee);
		}

		public Employee Get(Guid id)
		{
			return _employeeRepository.Get(id);
		}

		public void Update(Employee employee)
		{
			_employeeRepository.Update(employee);
		}
	}
}
