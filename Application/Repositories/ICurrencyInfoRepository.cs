using Domain.Entities;

namespace Application.Repositories;

public interface ICurrencyInfoRepository
{
	IAsyncEnumerable<Currency> GetAllCurrenciesAsync();
}