namespace MonoAccountProvider.Services.Models;

public record ProfileDto(
	IEnumerable<AccountDto> Accounts,
	IEnumerable<JarDto>? Jars);