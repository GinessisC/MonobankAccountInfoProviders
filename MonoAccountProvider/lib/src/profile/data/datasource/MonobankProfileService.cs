using System.Net.Http.Json;
using MonoAccountProvider.lib.src.profile.data.Models;

namespace MonoAccountProvider.lib.src.profile.data.datasource;

public class MonobankProfileService
{
	private const string ClientInfoUri = "https://api.monobank.ua/personal/client-info";
	private const string NameOfTokenUrlHeader = "X-Token";
	private readonly HttpClient _httpClient;

	public MonobankProfileService(HttpClient client)
	{
		_httpClient = client;
	}

	public async Task<ProfileDto> GetProfileAsync(string token)
	{
		_httpClient.DefaultRequestHeaders.Add(NameOfTokenUrlHeader, token);
		HttpResponseMessage response = await _httpClient.GetAsync(ClientInfoUri);

		if (response.IsSuccessStatusCode is false)
		{
			throw new ArgumentException("Unable to get profile. Token is invalid.");
		}

		// ! is used, bc anyway if status code is 200 - profile was received
		ProfileDto profileDto = (await response.Content.ReadFromJsonAsync<ProfileDto>()) !;

		return profileDto;
	}
}