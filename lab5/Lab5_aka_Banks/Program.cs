using System;
using System.Linq;

namespace Lab5_aka_Banks
{
	public class Program
	{
		private static (string, Action<CentralBank>)[] _mainMenuItems = new (string, Action<CentralBank>)[]
		{
			( "Banking", cb => RunMenu(cb, _bankingMenuItems) ),
			( "Print all", PrintAll ),
			( "Go ahead!", cb => GoAhead() ),
			( "Check time", cb => CheckTime() ),
			( "Exit", null )
		};

		private static (string, Action<CentralBank>)[] _bankingMenuItems = new (string, Action<CentralBank>)[]
		{
			( "Accounts", cb => RunMenu(cb, _accountsMenuItems) ),
			( "Clients", cb => RunMenu(cb, _clientsMenuItems) ),
			( "Print banks", PrintBanks ),	
			( "Put money", PutMoneyOnAccount ),
			( "Withdraw money", WithdrawMoneyFromAccount ),
			( "Create transaction", CreateTransaction ),
			( "Cancel transaction", CancelTransaction ),
			( "Back", null )
		};

		private static (string, Action<CentralBank>)[] _accountsMenuItems = new (string, Action<CentralBank>)[]
		{
			( "Create account", CreateAccountInBank ),
			( "Print accounts", PrintAccounts ),
			( "Print balance", PrintBalance ),
			( "Back", null )
		};

		private static (string, Action<CentralBank>)[] _clientsMenuItems = new (string, Action<CentralBank>)[]
		{
			( "Create client", CreateClientInBank ),
			( "Print clients", PrintClients ),
			( "Back", null )
		};

		public static void Main(string[] args)
		{
			var centralBank = new CentralBank();

			InitWithSamples(centralBank);
			RunMenu(centralBank, _mainMenuItems);
		}

		private static void RunMenu(CentralBank centralBank, (string caption, Action<CentralBank> action)[] menuItems)
		{
			while (true)
			{
				Console.Clear();

				var menuCaptions = menuItems.Select(i => i.caption).ToArray();
				PrintMenu(menuCaptions);

				var keyPressed = Console.ReadKey(true).KeyChar.ToString();

				if (!int.TryParse(keyPressed, out var keyInt) || keyInt > menuItems.Length)
				{
					continue;
				}

				var action = menuItems[keyInt - 1].action;

				if (action == null)
				{
					break;
				}

				Console.Clear();

				try
				{
					action(centralBank);
				}
				catch (Exception ex)
				{
					WriteError(ex.Message);
				}

				Console.ReadKey(true);
			}
		}

		private static void PrintMenu(string[] menuItems)
		{
			for (int i = 0; i < menuItems.Length; i++)
			{
				Console.WriteLine($"{i + 1}. {menuItems[i]}");
			}
		}

		private static void PrintAll(CentralBank centralBank)
		{
			var banks = centralBank.GetBanks();

			foreach (var bank in banks)
			{
				Console.WriteLine($"======{bank.Name} (ID: {bank.Id})======");

				var clients = bank.GetClients();

				foreach (var client in clients)
				{
					Console.WriteLine($"ID: {client.Id}, {client.FirstName} {client.LastName}, {client.Address}, {client.PassportId}");

					var accounts = bank.GetAccounts(client.Id);
					foreach (var account in accounts)
					{
						Console.WriteLine($"- ID: {account.id}, {account.balance}");
					}
				}

				Console.WriteLine();
			}
		}

		private static void PrintBanks(CentralBank centralBank)
		{
			var banks = centralBank.GetBanks();

			foreach (var bank in banks)
			{
				Console.WriteLine($"{bank.Name} (ID: {bank.Id})");
			}
		}

		private static Bank GetBank(CentralBank centralBank, string caption = null)
		{
			PrintBanks(centralBank);
			var bankId = Guid.Parse(Input(caption ?? "Bank id"));

			return centralBank.GetBank(bankId);
		}

		private static void CreateClientInBank(CentralBank centralBank)
		{
			var bank = GetBank(centralBank);
			var firstName = Input("First name");

			if (string.IsNullOrWhiteSpace(firstName))
			{
				WriteError("First name is required");
			}

			var lasttName = Input("Last name");

			if (string.IsNullOrWhiteSpace(firstName))
			{
				WriteError("Last name is required");
			}

			var address = Input("Address");
			var passportId = Input("Passport ID");

			var clientId = bank.CreateClient(firstName, lasttName, address, passportId);
			Console.WriteLine($"Client \"{clientId}\" created successfully!");
		}

		private static void CreateAccountInBank(CentralBank centralBank)
		{
			var bank = GetBank(centralBank);
			var clientId = Guid.Parse(Input("Client id"));
			var accountType = Input("Account type");

			Guid accountId;

			switch (accountType)
			{
				case "debit":
					accountId = bank.CreateDebitAccount(clientId);
					break;

				case "deposit":
					var term = TimeSpan.FromDays(int.Parse(Input("Deposit term (in days)")));
					var initialBalance = decimal.Parse(Input("Initial balance"));

					accountId = bank.CreateDepositAccount(clientId, term, initialBalance);
					break;

				case "credit":
					accountId = bank.CreateCreditAccount(clientId);
					break;

				default:
					WriteError("Invalid account type");
					return;
			}

			Console.WriteLine($"Account \"{accountId}\" created successfully!");
		}

		private static void PrintAccounts(CentralBank centralBank)
		{
			var bank = GetBank(centralBank);
			var clientId = Guid.Parse(Input("Client id"));

			var accounts = bank.GetAccounts(clientId);

			foreach (var account in accounts)
			{
				Console.WriteLine($"ID: {account.id}, balance: {account.balance:#.##}");
			}
		}

		private static void PrintClients(CentralBank centralBank)
		{
			var bank = GetBank(centralBank);
			var clients = bank.GetClients();

			foreach (var client in clients)
			{
				Console.WriteLine($"ID: {client.Id}, {client.FirstName} {client.LastName}, {client.Address}, {client.PassportId}");
			}
		}

		private static void PrintBalance(CentralBank centralBank)
		{
			var bank = GetBank(centralBank);
			var accountId = Guid.Parse(Input("Account id"));

			var balance = bank.GetBalance(accountId);

			if (balance == null)
			{
				Console.WriteLine("Operation failed!");
				return;
			}

			Console.WriteLine($"Balance: {balance:#.##}");
		}

		private static void PutMoneyOnAccount(CentralBank centralBank)
		{
			var bank = GetBank(centralBank);
			var accountId = Guid.Parse(Input("Account id"));
			var amount = decimal.Parse(Input("Amount"));

			if (bank.PutMoney(accountId, amount))
			{
				Console.WriteLine("Operation succeeded!");
				return;
			}

			Console.WriteLine("Operation failed!");
		}

		private static void WithdrawMoneyFromAccount(CentralBank centralBank)
		{
			var bank = GetBank(centralBank);
			var accountId = Guid.Parse(Input("Account id"));
			var amount = decimal.Parse(Input("Amount"));

			if (bank.WithdrawMoney(accountId, amount))
			{
				Console.WriteLine("Operation succeeded!");
				return;
			}

			Console.WriteLine("Operation failed!");
		}

		private static void CreateTransaction(CentralBank centralBank)
		{
			var sourceBank = GetBank(centralBank, "Source bank id");
			var sourceAccountId = Guid.Parse(Input("Source account id"));

			var targetBank = GetBank(centralBank, "Target bank id");
			var targetAccountId = Guid.Parse(Input("Target account id"));

			var amount = decimal.Parse(Input("Amount"));

			var transactionId = sourceBank.CreateTransaction(sourceAccountId, targetBank.Id, targetAccountId, amount);

			if (transactionId == null)
			{
				Console.WriteLine("Operation failed!");
				return;
			}

			Console.WriteLine($"Transaction \"{transactionId}\" created successfully!");
		}

		private static void CancelTransaction(CentralBank centralBank)
		{
			var bank = GetBank(centralBank);
			var transactionId = Guid.Parse(Input("Transaction id"));

			if (bank.CancelTransaction(transactionId))
			{
				Console.WriteLine($"Transaction \"{transactionId}\" canceled successfully!");
				return;
			}

			Console.WriteLine("Operation failed!");
		}

		private static void GoAhead()
		{
			var days = int.Parse(Input("Go ahead (in days)"));
			TimeMachine.GoAhead(days);

			CheckTime();
		}

		private static void CheckTime()
		{
			var nowString = TimeMachine.Now.ToString("dd.MM.yyyy HH.mm.ss");
			Console.WriteLine($"Current time is {nowString} (time is fixed)");
		}

		private static string Input(string caption)
		{
			Console.Write($"{caption}: ");
			return Console.ReadLine();
		}

		private static void WriteError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
		}

		private static void InitWithSamples(CentralBank centralBank)
		{
			var barclays = centralBank.RegisterBank(
				"Barclays",
				new BankConditions(
					debitAccountInterestPerAnnum: .0365,
					depositAccountInterestsPerAnnum: new[] { (0m, .03), (50_000, .035), (100_000, .04) },
					creditAccountComission: .05,
					creditAccountLimit: 300_000,
					maxAmountOfDoubtfulOperation: 100_000,
					dayOfInterestPutting: 1));

			var vtb = centralBank.RegisterBank(
				"VTB",
				new BankConditions(
					debitAccountInterestPerAnnum: .02,
					depositAccountInterestsPerAnnum: new[] { (10_000m, .05), (100_000, .06), (300_000, .08), (1_000_000, .1) },
					creditAccountComission: .045,
					creditAccountLimit: 800_000,
					maxAmountOfDoubtfulOperation: 50_000,
					dayOfInterestPutting: 10));

			var tinkoff = centralBank.RegisterBank(
				"Tinkoff",
				new BankConditions(
					debitAccountInterestPerAnnum: .05,
					depositAccountInterestsPerAnnum: new[] { (0m, .05), (50_000, .06), (200_000, .08) },
					creditAccountComission: .04,
					creditAccountLimit: 200_000,
					maxAmountOfDoubtfulOperation: 150_000,
					dayOfInterestPutting: 1));

			var test = centralBank.RegisterBank(
				"Test",
				new BankConditions(
					debitAccountInterestPerAnnum: .05,
					depositAccountInterestsPerAnnum: new[] { (0m, .05), (50_000, .06), (200_000, .08) },
					creditAccountComission: .04,
					creditAccountLimit: 200_000,
					maxAmountOfDoubtfulOperation: 150_000,
					dayOfInterestPutting: DateTime.Now.Day));

			var client1 = barclays.CreateClient("Tom", "Jahnson", "Lake ave. 10", "123456");
			var client2 = barclays.CreateClient("Adam", "Jahnson", "Lake ave. 24", "123457");

			var client3 = vtb.CreateClient("Sam", "Jackson", "Sharp road 9", "123458");
			var client4 = vtb.CreateClient("Adam", "Jahnson");

			var client5 = tinkoff.CreateClient("Tim", "Cook", "Sharp road 853", "123459");
			var client6 = tinkoff.CreateClient("Tom", "Jahnson");

			var client7 = test.CreateClient("Test", "Client", "Test road 01", "000001");

			var acc1 = barclays.CreateDebitAccount(client1);
			barclays.CreateDepositAccount(client1, TimeSpan.FromDays(180), 75_000);

			barclays.CreateDebitAccount(client2);
			barclays.CreateDepositAccount(client2, TimeSpan.FromDays(365), 20_000);
			barclays.CreateCreditAccount(client2);

			vtb.CreateDebitAccount(client3);
			vtb.CreateCreditAccount(client3);

			vtb.CreateDebitAccount(client4);
			vtb.CreateDepositAccount(client4, TimeSpan.FromDays(90), 3_500_000);
			vtb.CreateDepositAccount(client4, TimeSpan.FromDays(1095), 250_000);

			tinkoff.CreateCreditAccount(client5);
			tinkoff.CreateCreditAccount(client5);

			tinkoff.CreateDebitAccount(client6);

			test.CreateDepositAccount(client7, TimeSpan.FromDays(30), 500_000);

			barclays.PutMoney(acc1, 100_000);
		}
	}
}
