namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class Jar
{
	public string Title { get; }
	public Money OnBalance { get; }

	public Jar(string title,
		decimal balance,
		int currencyCode)
	{
		Title = title;
		OnBalance = new Money(balance, currencyCode);
	}
}