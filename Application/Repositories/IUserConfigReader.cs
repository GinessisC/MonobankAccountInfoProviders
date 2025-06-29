using Domain.Entities.DataSources;

namespace MonoAccountProvider.Domain.Repositories;

public interface IUserConfigReader
{
	UserCfgOptions Read();
}