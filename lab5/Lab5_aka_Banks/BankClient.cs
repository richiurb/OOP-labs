using System;

namespace Lab5_aka_Banks
{
	public class BankClient : IdentifiedEntity
	{
		public BankClient(string firstName, string lastName, string address, string passportId)
		{
			if (string.IsNullOrWhiteSpace(firstName))
			{
				throw new ArgumentException("First name must be set");
			}

			if (string.IsNullOrWhiteSpace(lastName))
			{
				throw new ArgumentException("Last name must be set");
			}

			FirstName = firstName;
			LastName = lastName;
			Address = address;
			PassportId = passportId;
		}

		public string FirstName { get; }

		public string LastName { get; }

		public string Address { get; }

		public string PassportId { get; }

		public bool IsDoubtful => string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(PassportId);
	}
}
