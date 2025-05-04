namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class UserAccountInCurrencies
{
	public string MaskedPan { get; }
	public IList<MoneyWithNamedCurrency> Balance { get; }

	public UserAccountInCurrencies(string maskedPan, IList<MoneyWithNamedCurrency> balance)
	{
		MaskedPan = maskedPan;
		Balance = balance;
	}
}