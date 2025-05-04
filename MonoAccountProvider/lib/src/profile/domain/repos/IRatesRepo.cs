using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.repos;

public interface IRatesRepo
{
	Task<IList<CurrencyRate>> GetCurrencyRatesAsync();
}