using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.usecases;

public class MoneyConverter
{
	private const int UahCurrencyCode = 980;

	private Money OnBalance { get; set; }
	private IList<CurrencyRate> CurrencyRates { get; }

	public MoneyConverter(IList<CurrencyRate> rates, Money balance)
	{
		OnBalance = balance;
		CurrencyRates = rates;
	}

	public IList<Money> ConvertTo(IList<int> targetCurrencyCodes)
	{
		List<Money> money = new();

		if (OnBalance.CurrencyCode != UahCurrencyCode)
		{
			OnBalance = ConvertTo(UahCurrencyCode);
		}

		foreach (int code in targetCurrencyCodes)
		{
			if (code == OnBalance.CurrencyCode)
			{
				money.Add(OnBalance);
			}
			else
			{
				Money convertedMoney = ConvertTo(code);
				money.Add(convertedMoney);
			}
		}

		return money;
	}

	private Money ConvertTo(int targetCurrencyCode)
	{
		Money convertedMoney = GetConvertedMoney(targetCurrencyCode);

		if (IsConversionSuccessful(convertedMoney) is false)
		{
			throw new ArgumentException($"Currencies code {OnBalance.CurrencyCode} could not found");
		}

		return convertedMoney;
	}

	private Money GetConvertedMoney(int targetCurrencyCode)
	{
		Money convertedMoney = new(0, 0);

		foreach (CurrencyRate cr in CurrencyRates)
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

		if (rate.CurrencyCodeA == OnBalance.CurrencyCode && rate.CurrencyCodeB == targetCurrencyCode)
		{
			amount = OnBalance.Amount * (rate.RateBuy ?? default);
		}

		if (rate.CurrencyCodeB == OnBalance.CurrencyCode && rate.CurrencyCodeA == targetCurrencyCode)
		{
			amount = OnBalance.Amount / (rate.RateSell ?? default);
		}

		return amount;
	}

	private decimal GetAmountWithRatesCross(CurrencyRate cr, int targetCurrencyCode)
	{
		decimal amount = 0;

		if (cr.CurrencyCodeA == OnBalance.CurrencyCode && cr.CurrencyCodeB == targetCurrencyCode)
		{
			amount = OnBalance.Amount * (cr.RateCross ?? default);
		}

		if (cr.CurrencyCodeB == OnBalance.CurrencyCode && cr.CurrencyCodeA == targetCurrencyCode)
		{
			amount = OnBalance.Amount / (cr.RateCross ?? default);
		}

		return amount;
	}
}