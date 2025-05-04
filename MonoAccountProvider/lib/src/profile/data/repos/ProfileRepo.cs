using MonoAccountProvider.lib.src.profile.data.datasource;
using MonoAccountProvider.lib.src.profile.data.Models;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.repos;

namespace MonoAccountProvider.lib.src.profile.data.repos;

public class ProfileRepo : IProfileRepo
{
	private readonly MonobankProfileService _service;

	public ProfileRepo(MonobankProfileService service)
	{
		_service = service;
	}

	public async Task<Profile> GetProfileAsync(string token)
	{
		ProfileDto profileDto = await _service.GetProfileAsync(token);

		return profileDto.FromDto();
	}
}