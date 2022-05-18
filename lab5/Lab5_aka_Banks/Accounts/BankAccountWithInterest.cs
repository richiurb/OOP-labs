using System;

namespace Lab5_aka_Banks.Accounts
{
	public abstract class BankAccountWithInterest : BankAccount
	{
		public BankAccountWithInterest(Guid bankId, Guid clientId, double interestPerAnnum) : base(bankId, clientId)
		{
			InterestPerAnnum = interestPerAnnum;
		}

		public double InterestPerAnnum { get; set; }

		public decimal InterestBalance { get; private set; }

		public void AccrueInterest()
		{
			if (!AllowInterestAccruing())
			{
				return;
			}

			InterestBalance += Balance * (decimal)(InterestPerAnnum / DaysInYear);
		}

		public void TakeAccruedInterest()
		{
			if (InterestBalance == 0)
			{
				return;
			}

			PutMoney(InterestBalance);
			InterestBalance = 0;
		}

		private int DaysInYear => DateTime.IsLeapYear(TimeMachine.Now.Year) ? 366 : 365;

		protected abstract bool AllowInterestAccruing();
	}
}
