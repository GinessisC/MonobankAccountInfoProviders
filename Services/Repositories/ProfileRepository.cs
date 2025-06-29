using Application.Repositories;
using Domain.Entities;
using MonoAccountProvider.Services.Mappings;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Services;

namespace MonoAccountProvider.Services.Repositories;

public class ProfileRepository : IProfileRepository
{
	private readonly MonobankProfileService _service;

	public ProfileRepository(MonobankProfileService service)
	{
		_service = service;
	}

	public async Task<Profile> GetProfileAsync(CancellationToken ct)
	{
		ProfileDto profileDto = await _service.GetProfileAsync(ct);

		return profileDto.ToProfile();
	}
}