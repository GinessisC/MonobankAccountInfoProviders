namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class Account
{
	public Money OnBalance { get; }
	public string MaskedPan { get; }

	public Account(decimal balance,
		int currencyCode,
		string maskedPan)
	{
		OnBalance = new Money(balance, currencyCode);
		MaskedPan = maskedPan;
	}
}