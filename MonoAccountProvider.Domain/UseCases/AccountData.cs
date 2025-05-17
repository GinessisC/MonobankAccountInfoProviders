using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.helpers.Interfaces;
using MonoAccountProvider.Domain.Repositories;

namespace MonoAccountProvider.Domain.UseCases;

public class AccountData : IAccountData
{
	private readonly IProfileRepository _profileRepository;
	private readonly IAsyncEnumerable<CurrencyRate> _rates;
	private readonly ICurrencyOperator _currencyOperator;
	private readonly UserConfig _userConfig;

	public AccountData(IProfileRepository profileRepository,
		ICurrencyOperator currencyOperator,
		IRatesRepository ratesRepo,
		IUserConfigReader userConfigReader)
	{
		_profileRepository = profileRepository;
		_rates = ratesRepo.GetCurrencyRatesAsync();
		_currencyOperator = currencyOperator;
		_userConfig = userConfigReader.Read();
	}

	public async IAsyncEnumerable<UserAccountInCurrencies> GetAccountsAsync()
	{
		Profile profile = await _profileRepository.GetProfileAsync(_userConfig.Token);

		IAsyncEnumerable<int> currencyCodes = _currencyOperator.ToCurrencyCodes(_userConfig.CurrencyNames);

		foreach (Account account in profile.Accounts)
		{
			MoneyConverter converter = new(_rates, account.OnBalance);
			IAsyncEnumerable<Money> moneyOnAccount = converter.ConvertTo(currencyCodes);

			IAsyncEnumerable<MoneyWithNamedCurrency> moneyWithNamedCurrencies =
				_currencyOperator.ToMoneyWithNamedCurrency(moneyOnAccount);

			yield return new UserAccountInCurrencies(account.MaskedPan, moneyWithNamedCurrencies);
		}
	}
}