using Domain.Entities;

namespace Application.Repositories;

public interface IProfileRepository
{
	Task<Profile> GetProfileAsync(CancellationToken ct);
}