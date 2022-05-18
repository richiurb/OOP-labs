using System;

namespace Lab5_aka_Banks.Accounts
{
	public class DebitAccount : BankAccountWithInterest
	{
		public DebitAccount(Guid bankId, Guid clientId, double interestPerAnnum) : base(bankId, clientId, interestPerAnnum)
		{
		}

		protected override bool AllowInterestAccruing() => true;

		protected override bool AllowWithdrawOperation(decimal amount) => Balance >= amount;
	}
}
