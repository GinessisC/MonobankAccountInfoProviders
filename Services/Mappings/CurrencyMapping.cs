using Domain.Entities;
using MonoAccountProvider.Services.Models;

namespace MonoAccountProvider.Services.Mappings;

public static class CurrencyMapping
{
	public static Currency ToCurrency(this CurrencyDto dto)
	{
		return new Currency(dto.Name, dto.CurrencyCode);
	}

	public static CurrencyRate ToCurrencyRate(this CurrencyRateDto dto)
	{
		return new CurrencyRate(dto.CurrencyCodeA, dto.CurrencyCodeB,
			dto.RateBuy,
			dto.RateSell,
			dto.RateCross);
	}
}