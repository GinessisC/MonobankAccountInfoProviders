using Microsoft.Extensions.Caching.Memory;

namespace MonoAccountProvider.Services.RequestHandlers;

public class CurrencyRatesRequestHandler : RequestHandler
{
	public CurrencyRatesRequestHandler(IMemoryCache memoryCache) 
		: base(memoryCache, new MemoryCacheEntryOptions()
	{
		Size = 10,
		AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
	})
	{
		
	}
}