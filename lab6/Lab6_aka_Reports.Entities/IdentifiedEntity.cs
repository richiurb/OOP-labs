using System;

namespace Lab6_aka_Reports.Entities
{
	public abstract class IdentifiedEntity
	{
		public IdentifiedEntity()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; }
	}
}
