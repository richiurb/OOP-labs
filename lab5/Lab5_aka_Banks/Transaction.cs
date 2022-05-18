using System;

namespace Lab5_aka_Banks
{
	public class Transaction : IdentifiedEntity
	{
		public Transaction(
			Guid sourceBankId, 
			Guid sourceAccountId,
			Guid targetBankId,
			Guid targetAccountId,
			decimal amount)
		{
			SourceBankId = sourceBankId;
			SourceAccountId = sourceAccountId;
			TargetBankId = targetBankId;
			TargetAccountId = targetAccountId;
			Amount = amount;
		}

		public Guid SourceBankId { get; }

		public Guid SourceAccountId { get; }

		public Guid TargetBankId { get; }

		public Guid TargetAccountId { get; }

		public decimal Amount { get; }
	}
}
