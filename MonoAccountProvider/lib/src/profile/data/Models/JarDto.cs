using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.data.Models;

public class JarDto
{
	public required string Title { get; set; }
	public required double Balance { get; set; }
	public required int CurrencyCode { get; set; }

	public Jar FromDto()
	{
		//balance is provided in coins. That's why we do balance / 100
		decimal newBalance = (decimal) Balance / 100;

		return new Jar(Title, newBalance, CurrencyCode);
	}
}