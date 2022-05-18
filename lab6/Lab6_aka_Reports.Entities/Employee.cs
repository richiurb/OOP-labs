using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab6_aka_Reports.Entities
{
	public class Employee : IdentifiedEntity
	{
		private Dictionary<Guid, Employee> _subordinates;

		public Employee(string name, Employee leader, IEnumerable<Employee> subordinates)
		{
			Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Name must be set", nameof(name));

			_subordinates = new Dictionary<Guid, Employee>();

			SetLeader(leader);

			if (subordinates != null)
			{
				foreach (var subordinate in subordinates)
				{
					AddSubordinate(subordinate);
				}
			}
		}

		public string Name { get; set; }

		public Employee Leader { get; private set; }

		public Employee[] Subordinates => _subordinates.Values.ToArray();

		public bool SetLeader(Employee employee)
		{
			if (Leader?.Id == employee?.Id || !CheckSubordinates(employee))
			{
				return false;
			}

			Leader?._subordinates.Remove(Id);
			employee?._subordinates.Add(Id, this);
			Leader = employee;
			return true;
		}

		public bool AddSubordinate(Employee employee)
		{
			if (employee == null || _subordinates.ContainsKey(employee.Id) || !CheckLeader(employee))
			{
				return false;
			}

			employee.Leader?._subordinates.Remove(employee.Id);
			employee.Leader = this;
			_subordinates.Add(employee.Id, employee);
			return true;
		}

		public bool RemoveSubordinate(Employee employee)
		{
			if (employee == null || employee.Subordinates.Any())
			{
				return false;
			}

			_subordinates.Remove(employee.Id);
			employee.Leader = null;
			return true;
		}

		private bool CheckLeader(Employee employee)
		{
			if (Leader == null)
			{
				return true;
			}

			if (Leader.Id == employee.Id)
			{
				return false;
			}

			return Leader.CheckLeader(employee);
		}

		private bool CheckSubordinates(Employee employee)
		{
			if (employee == null || !Subordinates.Any())
			{
				return true;
			}

			if (_subordinates.ContainsKey(employee.Id))
			{
				return false;
			}

			return _subordinates.All(s => s.Value.CheckSubordinates(employee));
		}
	}
}
