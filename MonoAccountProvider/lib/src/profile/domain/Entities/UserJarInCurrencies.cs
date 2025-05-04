namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class UserJarInCurrencies
{
	public string Title { get; }
	public IList<MoneyWithNamedCurrency> Balance { get; }

	public UserJarInCurrencies(string title, IList<MoneyWithNamedCurrency> balance)
	{
		Title = title;
		Balance = balance;
	}
}