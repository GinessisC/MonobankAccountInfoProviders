using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.data.Models;

public class CurrencyDto
{
	public string Cc { get; set; }
	public int R030 { get; set; }

	public CurrencyDto(string cc, int r030)
	{
		Cc = cc;
		R030 = r030;
	}

	public Currency FromDto()
	{
		return new Currency(Cc, R030);
	}
}