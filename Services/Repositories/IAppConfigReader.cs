namespace MonoAccountProvider.Services.Repositories;

public interface IAppConfigReader
{
	string GetSectionAsync(string sectionName);
}