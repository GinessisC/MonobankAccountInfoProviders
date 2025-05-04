using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.cache.Interfaces;

public interface ICurrencyInfoCache
{
	IList<CurrencyRate> Rates { get; }
	IList<Currency> Currencies { get; }
}