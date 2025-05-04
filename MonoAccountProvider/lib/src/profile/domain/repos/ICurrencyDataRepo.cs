using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.repos;

public interface ICurrencyDataRepo
{
	Task<IList<Currency>> GetAllCurrenciesAsync();
}