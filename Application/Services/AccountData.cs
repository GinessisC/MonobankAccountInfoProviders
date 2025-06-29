using Application.Repositories;
using Domain.Entities;
using Domain.Entities.DataSources;
using Microsoft.Extensions.Options;
using MonoAccountProvider.Domain.Helpers.Interfaces;
using MonoAccountProvider.Domain.Repositories;

namespace Application.Services;

public class AccountData : IAccountData
{
	private readonly IProfileRepository _profileRepository;
	private readonly IAsyncEnumerable<CurrencyRate> _rates;
	private readonly ICurrencyOperator _currencyOperator;
	private readonly UserCfgOptions _userCfgOptions;

	public AccountData(IProfileRepository profileRepository,
		ICurrencyOperator currencyOperator,
		IRatesRepository ratesRepo,
		IOptions<UserCfgOptions> userCfgOptionsReader)
	{
		_profileRepository = profileRepository;
		_rates = ratesRepo.GetCurrencyRatesAsync();
		_currencyOperator = currencyOperator;
		_userCfgOptions = userCfgOptionsReader.Value;
	}

	public async IAsyncEnumerable<UserAccountInCurrencies> GetAccountsAsync(CancellationToken ct)
	{
		Profile profile = await _profileRepository.GetProfileAsync(ct);

		IAsyncEnumerable<int> currencyCodes = _currencyOperator.ToCurrencyCodes(_userCfgOptions.CurrencyNames);

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