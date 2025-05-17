using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Services.Extensions;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Services;

namespace MonoAccountProvider.Services.Repos;

public class ProfileRepository : IProfileRepository
{
	private readonly MonobankProfileService _service;

	public ProfileRepository(MonobankProfileService service)
	{
		_service = service;
	}

	public async Task<Profile> GetProfileAsync(string token)
	{
		ProfileDto profileDto = await _service.GetProfileAsync(token);

		return profileDto.ToProfile();
	}
}