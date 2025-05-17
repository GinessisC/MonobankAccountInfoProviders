namespace MonoAccountProvider.Domain.Entities;

public record CurrencyRate(
	int CurrencyCodeA,
	int CurrencyCodeB,
	decimal? RateBuy,
	decimal? RateSell,
	decimal? RateCross);