using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Services.Models;

namespace MonoAccountProvider.Services.Extensions;

public static class ProfileMapping
{
	public static Profile ToProfile(this ProfileDto dto)
	{
		IEnumerable<Account> newAccounts = dto.Accounts.Select(ToAccount);

		if (dto.Jars is null)
		{
			return new Profile(newAccounts, null);
		}

		return new Profile(newAccounts, dto.Jars.Select(ToJar));
	}

	private static Account ToAccount(AccountDto dto)
	{
		Money onAccount = new(FromCoins(dto.Balance), dto.CurrencyCode);

		return new Account(
			dto.MaskedPan[0],
			onAccount);
	}

	private static Jar ToJar(JarDto dto)
	{
		Money onJar = new(FromCoins(dto.Balance), dto.CurrencyCode);

		return new Jar(
			dto.Title,
			onJar);
	}

	private static decimal FromCoins(decimal coins)
	{
		return coins / 100;
	}
}