using Microsoft.Extensions.Caching.Memory;

namespace MonoAccountProvider.Services.RequestHandlers;

public class MonobankProfileRequestHandler : RequestHandler
{
	public MonobankProfileRequestHandler(IMemoryCache memoryCache)
		: base(memoryCache, new MemoryCacheEntryOptions
		{
			Size = 5,
			AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
			Priority = CacheItemPriority.Low
		})
	{
	}
}