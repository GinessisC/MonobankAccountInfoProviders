using System.Globalization;
using ConsoleTables;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.repos;
using MonoAccountProvider.lib.src.profile.domain.usecases;

namespace MonoAccountProvider.lib.src.profile.presentation.view;

public class ConsolePresenter
{
	private readonly UserConfig _conf;
	private readonly IList<UserAccountInCurrencies> _accounts;
	private readonly IList<UserJarInCurrencies>? _jars;

	public ConsolePresenter(IConfigReader configReader,
		IAccountData accounts,
		IJarData jar)
	{
		_accounts = accounts.GetAccounts();
		_jars = jar.GetJars();
		_conf = configReader.Read();
	}

	public void Present()
	{
		ConsoleTable table = new("Name");
		table.AddColumn(_conf.CurrencyNames);

		AddAccountsRowTo(table);
		AddJarsRowTo(table);

		AddTotalRowTo(table);

		table.Write();
	}

	private void AddTotalRowTo(ConsoleTable table)
	{
		List<string> totalsWithTotalWordInStart = ["Total"];

		IEnumerable<string> totalsInString = GetTotalOnAccountAndJarInAllCurrencies()
			.Select(d => d.ToString());

		totalsWithTotalWordInStart.AddRange(totalsInString);

		table.AddRow(totalsWithTotalWordInStart.ToArray<object>());
	}

	private void AddAccountsRowTo(ConsoleTable table)
	{
		foreach (UserAccountInCurrencies account in _accounts)
		{
			List<string> maskedPanWithBalances = [];
			maskedPanWithBalances.Add(account.MaskedPan);

			AddFromAccountAmountsOfMoneyTo(maskedPanWithBalances, account);

			table.AddRow(maskedPanWithBalances.ToArray<object>());
		}
	}

	private void AddFromAccountAmountsOfMoneyTo(List<string> listOfAmounts, UserAccountInCurrencies account)
	{
		foreach (string currencyName in _conf.CurrencyNames)
		{
			decimal balanceInCurrency = account.Balance
				.Where(m => m.CurrencyName.ToString() == currencyName)
				.Select(m => m.Amount)
				.FirstOrDefault();

			listOfAmounts.Add(balanceInCurrency.ToString(CultureInfo.CurrentCulture));
		}
	}

	private void AddJarsRowTo(ConsoleTable table)
	{
		if (_jars == null)
		{
			Console.WriteLine("No Jars found");

			return;
		}

		foreach (UserJarInCurrencies? jar in _jars)
		{
			List<string> titleWithJarBalances = [];

			titleWithJarBalances.Add(jar.Title);

			AddFromJarAmountsOfMoneyTo(titleWithJarBalances, jar);

			table.AddRow(titleWithJarBalances.ToArray<object>());
		}
	}

	private void AddFromJarAmountsOfMoneyTo(List<string> listOfAmounts, UserJarInCurrencies jar)
	{
		foreach (string currencyName in _conf.CurrencyNames)
		{
			decimal balanceInCurrency = jar.Balance
				.Where(m => m.CurrencyName.ToString() == currencyName)
				.Select(m => m.Amount)
				.FirstOrDefault();

			listOfAmounts.Add(balanceInCurrency.ToString(CultureInfo.CurrentCulture));
		}
	}

	private List<decimal> GetTotalOnAccountAndJarInAllCurrencies()
	{
		List<decimal> totalOnAccountsAndJars = [];

		foreach (string currency in _conf.CurrencyNames)
		{
			decimal sumOnAccounts = GetAmountOnAccountsIn(currency);
			decimal sumOnJars = GetAmountOnJarsIn(currency);

			totalOnAccountsAndJars.Add(sumOnAccounts + sumOnJars);
		}

		return totalOnAccountsAndJars;
	}

	private decimal GetAmountOnAccountsIn(string currency)
	{
		decimal sum = 0;

		foreach (UserAccountInCurrencies account in _accounts)
		{
			decimal currentAccountAmount = account.Balance
				.Where(m => m.CurrencyName.ToString() == currency)
				.Select(m => m.Amount)
				.FirstOrDefault();

			sum += currentAccountAmount;
		}

		return sum;
	}

	private decimal GetAmountOnJarsIn(string currency)
	{
		decimal sum = 0;

		if (_jars == null)
		{
			return sum;
		}

		foreach (UserJarInCurrencies jar in _jars)
		{
			decimal currentJarAmount = jar.Balance
				.Where(m => m.CurrencyName.ToString() == currency)
				.Select(m => m.Amount)
				.FirstOrDefault();

			sum += currentJarAmount;
		}

		return sum;
	}
}