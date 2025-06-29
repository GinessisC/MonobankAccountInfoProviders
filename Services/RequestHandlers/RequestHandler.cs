using System.Net;
using Microsoft.Extensions.Caching.Memory;

namespace MonoAccountProvider.Services.RequestHandlers;

public abstract class RequestHandler : DelegatingHandler
{
	private readonly IMemoryCache _memoryCache;
	private readonly MemoryCacheEntryOptions _cacheOptions;

	protected RequestHandler(
		IMemoryCache memoryCache,
		MemoryCacheEntryOptions cacheEntryOptions)
	{
		_memoryCache = memoryCache;
		_cacheOptions = cacheEntryOptions;
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		string key = request.RequestUri?.ToString() ?? string.Empty;

		string content = await _memoryCache.GetOrCreateAsync(key, async entry =>
		{
			entry.SetOptions(_cacheOptions);

			HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

			return await response.Content.ReadAsStringAsync(cancellationToken);
		}, _cacheOptions) ?? throw new InvalidOperationException($"Cache miss on request: {key}");

		return new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(content)
		};
	}
}