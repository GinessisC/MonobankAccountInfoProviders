namespace MonoAccountProvider.Services.Models;

public record CurrencyRateDto(
	int CurrencyCodeA,
	int CurrencyCodeB,
	decimal? RateBuy,
	decimal? RateSell,
	decimal? RateCross);