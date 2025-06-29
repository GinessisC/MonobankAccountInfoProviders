using Domain.Entities;

namespace Application.Services;

public interface IAccountData
{
	IAsyncEnumerable<UserAccountInCurrencies> GetAccountsAsync(CancellationToken ct);
}