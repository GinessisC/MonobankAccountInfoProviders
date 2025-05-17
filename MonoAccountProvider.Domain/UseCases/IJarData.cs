using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.Domain.UseCases;

public interface IJarData
{
	IAsyncEnumerable<UserJarInCurrencies>? GetJars();
}