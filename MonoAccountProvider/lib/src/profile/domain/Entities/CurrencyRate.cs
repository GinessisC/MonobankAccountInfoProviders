namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class CurrencyRate
{
	public int CurrencyCodeA { get; }
	public int CurrencyCodeB { get; }
	public decimal? RateBuy { get; set; }
	public decimal? RateSell { get; set; }
	public decimal? RateCross { get; set; }

	public CurrencyRate(int currencyCodeA, int currencyCodeB)
	{
		CurrencyCodeA = currencyCodeA;
		CurrencyCodeB = currencyCodeB;
	}
}