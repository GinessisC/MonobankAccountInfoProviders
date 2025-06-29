using Domain.Entities;

namespace Application.Services;

public interface IJarData
{
	IAsyncEnumerable<UserJarInCurrencies>? GetJars(CancellationToken ct);
}