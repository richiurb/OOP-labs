using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5_aka_Banks
{
	public class BankConditions
	{
		public BankConditions(
			double debitAccountInterestPerAnnum,
			IEnumerable<(decimal minAmount, double interest)> depositAccountInterestsPerAnnum,
			double creditAccountComission,
			decimal creditAccountLimit,
			decimal maxAmountOfDoubtfulOperation,
			int dayOfInterestPutting)
		{
			DebitAccountInterestPerAnnum = debitAccountInterestPerAnnum;
			DepositAccountInterestsPerAnnum = depositAccountInterestsPerAnnum.OrderBy(i => i.minAmount).ToArray();
			CreditAccountComission = creditAccountComission;
			CreditAccountLimit = creditAccountLimit;
			MaxAmountOfDoubtfulOperation = maxAmountOfDoubtfulOperation;
			DayOfInterestPutting = dayOfInterestPutting;
		}

		public double DebitAccountInterestPerAnnum { get; }

		public (decimal minAmount, double interest)[] DepositAccountInterestsPerAnnum { get; }

		public double CreditAccountComission { get; }

		public decimal CreditAccountLimit { get; }

		public decimal MaxAmountOfDoubtfulOperation { get; }

		public int DayOfInterestPutting { get; }
	}
}
