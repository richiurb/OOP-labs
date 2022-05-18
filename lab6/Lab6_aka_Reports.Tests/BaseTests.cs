using System;
using Lab6_aka_Reports.Entities;

namespace Lab6_aka_Reports.Tests
{
	public class BaseTests
	{
		protected string GetRandomString() => Guid.NewGuid().ToString();

		protected Employee GetDanglingEmployee() => new Employee(GetRandomString(), null, null);
	}
}
