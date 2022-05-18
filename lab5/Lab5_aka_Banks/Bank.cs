using System;
using System.Collections.Generic;
using System.Linq;
using Lab5_aka_Banks.Accounts;

namespace Lab5_aka_Banks
{
	public class Bank : IdentifiedEntity
	{
		private CentralBank _centralBank;
		private Dictionary<Guid, BankAccount> _accounts;
		private Dictionary<Guid, BankClient> _clients;
		private DateTime? _lastInterestAccruing;

		public Bank(string name, BankConditions conditions, CentralBank centralBank)
		{
			if (conditions == null)
			{
				throw new ArgumentNullException(nameof(conditions));
			}

			Name = name;
			Conditions = conditions;

			_centralBank = centralBank;
			_accounts = new Dictionary<Guid, BankAccount>();
			_clients = new Dictionary<Guid, BankClient>();

			TimeMachine.OnDayChanged += (sender, date) => AccrueInterests(date);
		}

		public string Name { get; }

		public BankConditions Conditions { get; }

		public Guid CreateDebitAccount(Guid clientId) => 
			CreateBankAccount(clientId, () => new DebitAccount(Id, clientId, Conditions.DebitAccountInterestPerAnnum));

		public Guid CreateDepositAccount(Guid clientId, TimeSpan term, decimal initialBalance) =>
			CreateBankAccount(clientId, () =>  new DepositAccount(Id, clientId, term, initialBalance, GetDepositAccountInterest(initialBalance)));

		public Guid CreateCreditAccount(Guid clientId) => 
			CreateBankAccount(clientId, () => new CreditAccount(Id, clientId, Conditions.CreditAccountLimit, Conditions.CreditAccountComission));

		public Guid CreateClient(string firstName, string lastName, string address = null, string passportId = null)
		{
			var client = new BankClient(firstName, lastName, address, passportId);
			_clients.Add(client.Id, client);

			return client.Id;
		}

		public bool PutMoney(Guid accountId, decimal amount)
		{
			var account = GetAccount(accountId);
			return account != null && account.PutMoney(amount);
		}

		public bool WithdrawMoney(Guid accountId, decimal amount)
		{
			var account = GetAccount(accountId);

			if (account == null || !CheckDoubtfulOperationForAccount(account, amount))
			{
				return false;
			}

			return account.WithdrawMoney(amount);
		}

		public Guid? CreateTransaction(Guid sourceAccountId, Guid targetBankId, Guid targetAccountId, decimal amount)
		{
			var account = GetAccount(sourceAccountId);

			if (account == null || !CheckDoubtfulOperationForAccount(account, amount))
			{
				return null;
			}

			return _centralBank.CreateTransaction(Id, sourceAccountId, targetBankId, targetAccountId, amount);
		}

		public bool CancelTransaction(Guid transactionId)
		{
			return _centralBank.CancelTransaction(transactionId, Id);
		}

		public (Guid id, decimal balance)[] GetAccounts(Guid clientId)
		{
			return _accounts.Values
				.Where(a => a.ClientId == clientId)
				.Select(a => (a.Id, a.Balance))
				.ToArray();
		}

		public BankClient[] GetClients()
		{
			return _clients.Values.ToArray();
		}

		public decimal? GetBalance(Guid accountId)
		{
			return GetAccount(accountId)?.Balance;
		}

		private void AccrueInterests(DateTime date)
		{
			if (_lastInterestAccruing >= date)
			{
				return;
			}

			var accountsWithInterest = _accounts.Values.OfType<BankAccountWithInterest>();

			foreach (var account in accountsWithInterest)
			{
				account.AccrueInterest();

				if (date.Day == Conditions.DayOfInterestPutting)
				{
					account.TakeAccruedInterest();
				}
			}

			_lastInterestAccruing = date;
		}

		private Guid CreateBankAccount(Guid clientId, Func<BankAccount> creationFunction)
		{
			if (!_clients.ContainsKey(clientId))
			{
				throw new ArgumentException($"Client with id \"{clientId}\" does not exist");
			}

			var account = creationFunction();
			_accounts.Add(account.Id, account);

			return account.Id;
		}

		private double GetDepositAccountInterest(decimal initialAmount)
		{
			var interests = Conditions.DepositAccountInterestsPerAnnum;

			for (int i = 0; i < interests.Length; i++)
			{
				if (interests[i].minAmount > initialAmount)
				{
					return i > 0 ? interests[i - 1].interest : 0;
				}
			}

			return interests.Last().interest;
		}

		private bool CheckDoubtfulOperationForAccount(BankAccount account, decimal amount)
		{
			var client = GetClient(account.ClientId);
			return client != null && (!client.IsDoubtful || amount <= Conditions.MaxAmountOfDoubtfulOperation);
		}

		private BankAccount GetAccount(Guid accountId)
		{
			_accounts.TryGetValue(accountId, out var account);
			return account;
		}

		private BankClient GetClient(Guid clientId)
		{
			_clients.TryGetValue(clientId, out var client);
			return client;
		}
	}
}
