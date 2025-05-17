namespace MonoAccountProvider.Services.Repos;

public interface IAppConfigReader
{
	string GetSectionAsync(string sectionName);
}