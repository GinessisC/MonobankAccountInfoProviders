using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.usecases;

public interface IAccountData
{
	IList<UserAccountInCurrencies> GetAccounts();
}