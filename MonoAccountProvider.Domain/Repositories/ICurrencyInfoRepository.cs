using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.Domain.Repositories;

public interface ICurrencyInfoRepository
{
	IAsyncEnumerable<Currency> GetAllCurrenciesAsync();
}