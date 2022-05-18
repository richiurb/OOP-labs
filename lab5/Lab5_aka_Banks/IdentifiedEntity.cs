using System;

namespace Lab5_aka_Banks
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
