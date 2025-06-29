using Domain.Entities;

namespace Domain.Extensions;

public static class MoneyExtension
{
	public static async Task<MoneyWithNamedCurrency> ToMoneyWithNamedCurrencyAsync(
		this Money money,
		IAsyncEnumerable<Currency> currencies)
	{
		string? currencyName = await currencies
			.Where(c => c.Code == money.CurrencyCode)
			.Select(c => c.Name)
			.FirstOrDefaultAsync();

		if (currencyName is null)
		{
			throw new FormatException("No currency found");
		}

		return new MoneyWithNamedCurrency(money.Amount, currencyName);
	}
}