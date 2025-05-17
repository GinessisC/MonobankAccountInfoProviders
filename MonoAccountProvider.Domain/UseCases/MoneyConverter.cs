using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.Domain.UseCases;

public class MoneyConverter
{
	private const int UahCurrencyCode = 980;
	private readonly IAsyncEnumerable<CurrencyRate> _currencyRates;

	private Money _onBalance;

	public MoneyConverter(IAsyncEnumerable<CurrencyRate> rates, Money balance)
	{
		_onBalance = balance;
		_currencyRates = rates;
	}

	public async IAsyncEnumerable<Money> ConvertTo(IAsyncEnumerable<int> targetCurrencyCodes)
	{
		if (_onBalance.CurrencyCode != UahCurrencyCode)
		{
			_onBalance = await ConvertTo(UahCurrencyCode);
		}

		await foreach (int code in targetCurrencyCodes)
		{
			if (code == _onBalance.CurrencyCode)
			{
				yield return _onBalance;
			}
			else
			{
				Money convertedMoney = await ConvertTo(code);

				yield return convertedMoney;
			}
		}
	}

	private async Task<Money> ConvertTo(int targetCurrencyCode)
	{
		Money convertedMoney = await GetConvertedMoney(targetCurrencyCode);

		if (IsConversionSuccessful(convertedMoney) is false)
		{
			throw new ArgumentException($"Currencies code {_onBalance.CurrencyCode} could not found");
		}

		return convertedMoney;
	}

	private async Task<Money> GetConvertedMoney(int targetCurrencyCode)
	{
		Money convertedMoney = new(0, 0);

		await foreach (CurrencyRate cr in _currencyRates)
		{
			decimal amount = 0;

			if (cr.RateBuy != null && cr.RateSell != null)
			{
				amount = GetAmountWithRatesSellAndBuy(cr, targetCurrencyCode);
			}

			if (cr.RateCross != null)
			{
				amount = GetAmountWithRatesCross(cr, targetCurrencyCode);
			}

			if (amount != 0)
			{
				convertedMoney = new Money(amount, targetCurrencyCode);

				break;
			}
		}

		return convertedMoney;
	}

	private bool IsConversionSuccessful(Money money)
	{
		return money.CurrencyCode != 0;
	}

	private decimal GetAmountWithRatesSellAndBuy(CurrencyRate rate, int targetCurrencyCode)
	{
		decimal amount = 0;

		if (rate.CurrencyCodeA == _onBalance.CurrencyCode && rate.CurrencyCodeB == targetCurrencyCode)
		{
			amount = _onBalance.Amount * (rate.RateBuy ?? default);
		}

		if (rate.CurrencyCodeB == _onBalance.CurrencyCode && rate.CurrencyCodeA == targetCurrencyCode)
		{
			amount = _onBalance.Amount / (rate.RateSell ?? default);
		}

		return amount;
	}

	private decimal GetAmountWithRatesCross(CurrencyRate cr, int targetCurrencyCode)
	{
		decimal amount = 0;

		if (cr.CurrencyCodeA == _onBalance.CurrencyCode && cr.CurrencyCodeB == targetCurrencyCode)
		{
			amount = _onBalance.Amount * (cr.RateCross ?? default);
		}

		if (cr.CurrencyCodeB == _onBalance.CurrencyCode && cr.CurrencyCodeA == targetCurrencyCode)
		{
			amount = _onBalance.Amount / (cr.RateCross ?? default);
		}

		return amount;
	}
}