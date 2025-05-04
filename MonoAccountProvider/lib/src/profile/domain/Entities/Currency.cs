namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class Currency
{
	public string Name { get; }
	public int Code { get; }

	public Currency(string name, int code)
	{
		Name = name;
		Code = code;
	}
}