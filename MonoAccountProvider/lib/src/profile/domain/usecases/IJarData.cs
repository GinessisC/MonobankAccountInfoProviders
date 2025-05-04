using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.usecases;

public interface IJarData
{
	IList<UserJarInCurrencies>? GetJars();
}