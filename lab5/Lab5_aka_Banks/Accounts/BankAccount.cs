using System;

namespace Lab5_aka_Banks.Accounts
{
	public abstract class BankAccount : IdentifiedEntity
	{
		public BankAccount(Guid bankId, Guid clientId)
		{
			BankId = bankId;
			ClientId = clientId;
		}

		public Guid BankId { get; }

		public Guid ClientId { get; }

		public decimal Balance { get; private set; }

		public bool PutMoney(decimal amount)
		{
			if (amount <= 0)
			{
				return false;
			}

			Balance += amount;
			return true;
		}

		public bool WithdrawMoney(decimal amount)
		{
			if (amount <= 0 || !AllowWithdrawOperation(amount))
			{
				return false;
			}

			Balance -= amount;
			return true;
		}

		protected abstract bool AllowWithdrawOperation(decimal amount);
	}
}
