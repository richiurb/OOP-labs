using System;
using System.Linq;
using Lab6_aka_Reports.Entities;
using NUnit.Framework;

namespace Lab6_aka_Reports.Tests
{
	[TestFixture]
	public class EmployeeTests : BaseTests
	{
		#region Constructor

		[Test]
		public void Should_not_create_employee_with_no_name()
		{
			Assert.Throws<ArgumentException>(() => new Employee(null, null, null));
		}

		[Test]
		public void Should_create_employee_with_name()
		{
			var name = GetRandomString();
			var employee = new Employee(name, null, null);

			Assert.AreEqual(name, employee.Name);
		}

		[Test]
		public void Should_create_employee_with_leader()
		{
			var leader = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), leader, null);

			Assert.AreEqual(leader, employee.Leader);
			Assert.AreEqual(1, leader.Subordinates.Length);
			Assert.AreEqual(employee, leader.Subordinates.First());
		}

		[Test]
		public void Should_create_employee_with_subordinates()
		{
			var subordinate = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), null, new[] { subordinate });

			Assert.AreEqual(1, employee.Subordinates.Length);
			Assert.AreEqual(subordinate, employee.Subordinates.First());
		}

		#endregion

		#region SetLeader

		[Test]
		public void Should_set_leader()
		{
			var leader = GetDanglingEmployee();
			var employee = GetDanglingEmployee();

			var result = employee.SetLeader(leader);

			Assert.IsTrue(result);
			Assert.AreEqual(leader, employee.Leader);
			Assert.AreEqual(1, leader.Subordinates.Length);
			Assert.AreEqual(employee, leader.Subordinates.First());
		}

		[Test]
		public void Should_not_set_leader_if_same_leader_already_set()
		{
			var leader = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), leader, null);

			var result = employee.SetLeader(leader);

			Assert.IsFalse(result);
			Assert.AreEqual(leader, employee.Leader);
			Assert.AreEqual(1, leader.Subordinates.Length);
			Assert.AreEqual(employee, leader.Subordinates.First());
		}

		[Test]
		public void Should_set_leader_if_different_leader_already_set()
		{
			var leader1 = GetDanglingEmployee();
			var leader2 = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), leader1, null);

			var result = employee.SetLeader(leader2);

			Assert.IsTrue(result);
			Assert.AreEqual(leader2, employee.Leader);
			Assert.AreEqual(1, leader2.Subordinates.Length);
			Assert.AreEqual(0, leader1.Subordinates.Length);
			Assert.AreEqual(employee, leader2.Subordinates.First());
		}

		[Test]
		public void Should_not_set_leader_if_leader_is_subordinate()
		{
			var subordinate = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), null, new[] { subordinate });

			var result = employee.SetLeader(subordinate);

			Assert.IsFalse(result);
			Assert.AreEqual(null, employee.Leader);
			Assert.AreEqual(0, subordinate.Subordinates.Length);
		}

		#endregion

		#region AddSubordinate

		[Test]
		public void Should_add_subordinate()
		{
			var subordinate = GetDanglingEmployee();
			var employee = GetDanglingEmployee();

			var result = employee.AddSubordinate(subordinate);

			Assert.IsTrue(result);
			Assert.AreEqual(employee, subordinate.Leader);
			Assert.AreEqual(1, employee.Subordinates.Length);
			Assert.AreEqual(subordinate, employee.Subordinates.First());
		}

		[Test]
		public void Should_not_add_subordinate_if_same_subordinate_already_added()
		{
			var subordinate = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), null, new[] { subordinate });

			var result = employee.AddSubordinate(subordinate);

			Assert.IsFalse(result);
			Assert.AreEqual(1, employee.Subordinates.Length);
			Assert.AreEqual(subordinate, employee.Subordinates.First());
		}

		[Test]
		public void Should_add_subordinate_if_different_subordinate_already_added()
		{
			var subordinate1 = GetDanglingEmployee();
			var subordinate2 = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), null, new[] { subordinate1 });

			var result = employee.AddSubordinate(subordinate2);

			Assert.IsTrue(result);
			Assert.IsTrue(employee.Subordinates.Contains(subordinate1));
			Assert.IsTrue(employee.Subordinates.Contains(subordinate2));
			Assert.AreEqual(2, employee.Subordinates.Length);
			Assert.AreEqual(employee, subordinate1.Leader);
			Assert.AreEqual(employee, subordinate2.Leader);
		}

		[Test]
		public void Should_not_add_subordinate_if_subordinate_is_leader()
		{
			var leader = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), leader, null);

			var result = employee.AddSubordinate(leader);

			Assert.IsFalse(result);
			Assert.AreEqual(null, leader.Leader);
			Assert.AreEqual(0, employee.Subordinates.Length);
		}

		[Test]
		public void Should_not_add_subordinate_if_subordinate_is_null()
		{
			var employee = GetDanglingEmployee();

			var result = employee.AddSubordinate(null);

			Assert.IsFalse(result);
			Assert.AreEqual(0, employee.Subordinates.Length);
		}

		#endregion

		#region RemoveSubordinate

		[Test]
		public void Should_remove_subordinate()
		{
			var subordinate = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), null, new[] { subordinate });

			var result = employee.RemoveSubordinate(subordinate);

			Assert.IsTrue(result);
			Assert.AreEqual(0, employee.Subordinates.Length);
			Assert.AreEqual(null, subordinate.Leader);
		}

		[Test]
		public void Should_not_remove_subordinate_if_subordinate_is_null()
		{
			var subordinate = GetDanglingEmployee();
			var employee = new Employee(GetRandomString(), null, new[] { subordinate });

			var result = employee.RemoveSubordinate(null);

			Assert.IsFalse(result);
			Assert.AreEqual(1, employee.Subordinates.Length);
		}

		[Test]
		public void Should_not_remove_subordinate_if_subordinate_has_subordinates()
		{
			var subordinate1 = GetDanglingEmployee();
			var subordinate2 = new Employee(GetRandomString(), null, new[] { subordinate1 });
			var employee = new Employee(GetRandomString(), null, new[] { subordinate2 });

			var result = employee.RemoveSubordinate(subordinate2);

			Assert.IsFalse(result);
			Assert.AreEqual(1, employee.Subordinates.Length);
			Assert.AreEqual(subordinate2, employee.Subordinates.First());
		}

		#endregion
	}
}
