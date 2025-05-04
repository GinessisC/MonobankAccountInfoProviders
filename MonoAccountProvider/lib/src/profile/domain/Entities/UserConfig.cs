namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class UserConfig
{
	public string Token { get; }
	public string[] CurrencyNames { get; }

	public UserConfig(string token, string[] currencyNames)
	{
		Token = token;
		CurrencyNames = currencyNames;
	}
}