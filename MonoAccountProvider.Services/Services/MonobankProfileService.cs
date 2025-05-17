using System.Net.Http.Json;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Repos;

namespace MonoAccountProvider.Services.Services;

public class MonobankProfileService
{
	private const string NameOfTokenUrlHeader = "X-Token";
	private readonly HttpClient _httpClient;
	private readonly IAppConfigReader _appConfigReader;

	public MonobankProfileService(HttpClient client,
		IAppConfigReader appConfigReader)
	{
		_httpClient = client;
		_appConfigReader = appConfigReader;
	}

	public async Task<ProfileDto> GetProfileAsync(string token)
	{
		string clientInfoUri = _appConfigReader.GetSectionAsync("ProfileUri");

		_httpClient.DefaultRequestHeaders.Add(NameOfTokenUrlHeader, token);
		HttpResponseMessage response = await _httpClient.GetAsync(clientInfoUri);

		if (response.IsSuccessStatusCode is false)
		{
			throw new HttpRequestException($"fail to retrieve data from {clientInfoUri}." +
				$" Status Code: {response.StatusCode}." +
				$" {response.ReasonPhrase}");
		}

		// ! is used, bc anyway if status code is 200 - profile was received
		ProfileDto profileDto = (await response.Content.ReadFromJsonAsync<ProfileDto>()) !;

		return profileDto;
	}
}