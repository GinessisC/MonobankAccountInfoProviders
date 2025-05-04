using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.data.Models;

public class AccountDto
{
	public required double Balance { get; set; }
	public required int CurrencyCode { get; set; }
	public required IList<string> MaskedPan { get; set; }

	public Account FromDto()
	{
		return new Account(
			(decimal) Balance / 100, //balance is provided in coins. That's why we do balance / 100
			CurrencyCode,
			MaskedPan[0]);
	}
}