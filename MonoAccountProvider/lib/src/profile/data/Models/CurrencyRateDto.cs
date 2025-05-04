using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.data.Models;

public class CurrencyRateDto
{
	public required int CurrencyCodeA { get; set; }
	public required int CurrencyCodeB { get; set; }
	public double? RateBuy { get; set; }
	public double? RateSell { get; set; }
	public double? RateCross { get; set; }

	public CurrencyRate FromDto()
	{
		return new CurrencyRate(CurrencyCodeA, CurrencyCodeB)
		{
			RateBuy = ToDecimal(RateBuy),
			RateSell = ToDecimal(RateSell),
			RateCross = ToDecimal(RateCross)
		};
	}

	private decimal? ToDecimal(double? value)
	{
		if (value != null)
		{
			return (decimal) value;
		}

		return null;
	}
}