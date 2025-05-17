using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.Domain.Repositories;

public interface IUserConfigReader
{
	UserConfig Read();
}