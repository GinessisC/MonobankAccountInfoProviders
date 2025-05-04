using MonoAccountProvider.lib.src.profile.domain.cache.Interfaces;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.repos;
using Entities_UserConfig = MonoAccountProvider.lib.src.profile.domain.Entities.UserConfig;
using UserConfig = MonoAccountProvider.lib.src.profile.domain.Entities.UserConfig;

namespace MonoAccountProvider.lib.src.profile.domain.cache;

public class UserCache : IUserCache
{
	public Profile Profile { get; }
	public Entities_UserConfig Conf { get; }

	private UserCache(Profile profile, Entities_UserConfig conf)
	{
		Profile = profile;
		Conf = conf;
	}

	public static async Task<UserCache> CreateAsync(IProfileRepo profileRepo, IConfigReader confReader)
	{
		UserConfig conf = confReader.Read();
		Profile profile = await profileRepo.GetProfileAsync(conf.Token);

		return new UserCache(profile, conf);
	}
}