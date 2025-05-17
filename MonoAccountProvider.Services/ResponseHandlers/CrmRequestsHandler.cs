using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MonoAccountProvider.Services.ResponseHandlers;

public class CrmRequestsHandler : DelegatingHandler
{
	private readonly ILogger<CrmRequestsHandler> _logger;
	private readonly IMemoryCache _memoryCache;
	private readonly MemoryCacheEntryOptions _cacheOptions;

	public CrmRequestsHandler(ILogger<CrmRequestsHandler> logger,
		IMemoryCache memoryCache)
	{
		_logger = logger;
		_memoryCache = memoryCache;

		_cacheOptions = new MemoryCacheEntryOptions
		{
			Size = 1,
			AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
		};
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		string key = request.RequestUri?.ToString() ?? string.Empty;

		if (_memoryCache.TryGetValue(key, out string? cachedContent))
		{
			_logger.LogTrace("Returning cached string content");

			if (cachedContent != null)
			{
				return new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(cachedContent)
				};
			}
		}

		HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
		string content = await response.Content.ReadAsStringAsync(cancellationToken);

		_memoryCache.Set(key, content, _cacheOptions);

		return response;
	}
}