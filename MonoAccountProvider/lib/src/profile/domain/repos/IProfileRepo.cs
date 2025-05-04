using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.repos;

public interface IProfileRepo
{
	Task<Profile> GetProfileAsync(string token);
}