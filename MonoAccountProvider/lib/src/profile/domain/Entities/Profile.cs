namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class Profile
{
	public required IList<Account> Accounts { get; init; }
	public IList<Jar>? Jars { get; set; }
}