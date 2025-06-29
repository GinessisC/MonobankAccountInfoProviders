using Domain.Entities;

namespace Application.Repositories;

public interface IRatesRepository
{
	IAsyncEnumerable<CurrencyRate> GetCurrencyRatesAsync();
}