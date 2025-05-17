using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.Domain.Repositories;

public interface IProfileRepository
{
	Task<Profile> GetProfileAsync(string token);
}