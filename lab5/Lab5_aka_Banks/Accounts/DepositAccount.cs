using System;

namespace Lab5_aka_Banks.Accounts
{
	public class DepositAccount : BankAccountWithInterest
	{
		public DepositAccount(
			Guid bankId,
			Guid clientId,
			TimeSpan term,
			decimal initialBalance,
			double interestPerAnnum) : base(bankId, clientId, interestPerAnnum)
		{
			EndsOn = TimeMachine.Now.Add(term);
			PutMoney(initialBalance);
		}

		public DateTime EndsOn { get; }

		public bool IsEnded => EndsOn < TimeMachine.Now;

		protected override bool AllowInterestAccruing() => !IsEnded;

		protected override bool AllowWithdrawOperation(decimal amount) => IsEnded && Balance >= amount;
	}
}
