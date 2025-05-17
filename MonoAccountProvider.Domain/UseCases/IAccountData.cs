using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.Domain.UseCases;

public interface IAccountData
{
	IAsyncEnumerable<UserAccountInCurrencies> GetAccountsAsync();
}