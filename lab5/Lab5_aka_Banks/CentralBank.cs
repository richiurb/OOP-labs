using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5_aka_Banks
{
	public class CentralBank
	{
		private Dictionary<Guid, Bank> _banks;
		private Dictionary<Guid, Transaction> _transactions;

		public CentralBank()
		{
			_banks = new Dictionary<Guid, Bank>();
			_transactions = new Dictionary<Guid, Transaction>();
		}

		public Bank RegisterBank(string name, BankConditions conditions)
		{
			var bank = new Bank(name, conditions, this);
			_banks.Add(bank.Id, bank);

			return bank;
		}

		public Bank[] GetBanks() => _banks.Values.ToArray();

		public Bank GetBank(Guid bankId)
		{
			_banks.TryGetValue(bankId, out var bank);
			return bank;
		}

		public Guid? CreateTransaction(Guid sourceBankId, Guid sourceAccountId, Guid targetBankId, Guid targetAccountId, decimal amount)
		{
			var transaction = new Transaction(sourceBankId, sourceAccountId, targetBankId, targetAccountId, amount);
			
			var sourceBank = GetBank(sourceBankId);
			var targetBank = GetBank(targetBankId);

			if (sourceBank == null || targetBank == null)
			{
				return null;
			}

			var success = sourceBank.WithdrawMoney(sourceAccountId, amount)
				&& targetBank.PutMoney(targetAccountId, amount);

			if (success)
			{
				
				_transactions.Add(transaction.Id, transaction);
			}

			return transaction.Id;
		}

		public bool CancelTransaction(Guid transactionId, Guid requestingBankId)
		{
			var transaction = GetTransaction(transactionId);

			if (transaction == null || (transaction.SourceBankId != requestingBankId && transaction.TargetBankId != requestingBankId))
			{
				return false;
			}

			var revertedTransactionId = CreateTransaction(
				transaction.TargetBankId,
				transaction.TargetAccountId,
				transaction.SourceBankId,
				transaction.SourceAccountId,
				transaction.Amount);

			return revertedTransactionId != null;
		}

		private Transaction GetTransaction(Guid transactionId)
		{
			_transactions.TryGetValue(transactionId, out var transaction);
			return transaction;
		}
	}
}
