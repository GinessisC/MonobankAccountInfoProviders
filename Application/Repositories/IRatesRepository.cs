using Domain.Entities;

namespace MonoAccountProvider.Domain.Repositories;

public interface IRatesRepository
{
	IAsyncEnumerable<CurrencyRate> GetCurrencyRatesAsync();
}