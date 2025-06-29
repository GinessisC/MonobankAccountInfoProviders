using Microsoft.Extensions.Caching.Memory;

namespace MonoAccountProvider.Services.RequestHandlers;

public class CurrencyInfoRequestHandler : RequestHandler
{
	public CurrencyInfoRequestHandler(IMemoryCache memoryCache)
		: base(memoryCache, new MemoryCacheEntryOptions
		{
			Size = 10,
			Priority = CacheItemPriority.NeverRemove
		})
	{
	}
}