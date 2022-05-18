using System;
using Lab6_aka_Reports.Entities.Reports;

namespace Lab6_aka_Reports.DAL
{
	public interface ISprintReportRepository
	{
		void Insert(SprintReport report);

		SprintReport Get(Guid employeeId, DateTime fromDate, DateTime toDate);

		void Update(SprintReport report);
	}
}
