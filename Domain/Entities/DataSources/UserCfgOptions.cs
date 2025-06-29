namespace Domain.Entities.DataSources;

public record UserCfgOptions
{
	public string Token { get; set; }
	public IEnumerable<string> CurrencyNames { get; set; }
}