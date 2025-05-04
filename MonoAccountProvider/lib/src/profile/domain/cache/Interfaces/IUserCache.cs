using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.cache.Interfaces;

public interface IUserCache
{
	public Profile Profile { get; }
	public UserConfig Conf { get; }
}