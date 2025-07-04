namespace Domain.Entities;

public record Profile(
	IEnumerable<Account> Accounts,
	IEnumerable<Jar>? Jars);