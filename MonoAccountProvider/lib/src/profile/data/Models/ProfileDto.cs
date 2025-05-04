using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.data.Models;

public class ProfileDto
{
	public required IList<AccountDto> Accounts { get; set; }
	public IList<JarDto>? Jars { get; set; }

	public Profile FromDto()
	{
		List<Account> newAccounts = Accounts.Select(a => a.FromDto()).ToList();

		if (Jars is null)
		{
			return new Profile
			{
				Accounts = newAccounts
			};
		}

		return new Profile
		{
			Accounts = newAccounts,
			Jars = Jars.Select(j => j.FromDto()).ToList()
		};
	}
}