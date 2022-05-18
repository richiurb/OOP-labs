using System;

namespace Lab5_aka_Banks.Accounts
{
	public class CreditAccount : BankAccount
	{
		public CreditAccount(Guid bankId, Guid clientId, decimal limit, double comission) : base(bankId, clientId)
		{
			Limit = limit;
			Comission = comission;
		}

		public decimal Limit { get; }

		public double Comission { get; }

		protected override bool AllowWithdrawOperation(decimal amount) => Balance - (Balance < 0 ? ApplyComission(amount) : amount) <= -Limit;

		private decimal ApplyComission(decimal amount) => amount * (decimal)(1 - Comission);
	}
}
