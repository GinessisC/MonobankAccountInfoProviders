using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Domain.Entities;
using Domain.Entities.DataSources;
using MonoAccountProvider.Services.Models;

namespace MonoAccountProvider.Services.Services;

public class MonobankProfileService
{
	private const string NameOfTokenUrlHeader = "X-Token";
	private readonly HttpClient _httpClient;
	private readonly ProfileSourceOptions _profileSourceOptions;
	private readonly string _token;
	
	public MonobankProfileService(HttpClient client,
		IOptions<ProfileSourceOptions> profileOptions,
		IOptionsSnapshot<UserCfgOptions> userOptions)
	{
		_httpClient = client;
		_profileSourceOptions = profileOptions.Value; 
		_token = userOptions.Value.Token;
	}
	public async Task<ProfileDto> GetProfileAsync(CancellationToken ct)
	{
		string clientInfoUri = _profileSourceOptions.Uri;

		_httpClient.DefaultRequestHeaders.Add(NameOfTokenUrlHeader, _token);
		HttpResponseMessage response = await _httpClient.GetAsync(clientInfoUri, ct);

		if (response.IsSuccessStatusCode is false)
		{
			throw new HttpRequestException($"fail to retrieve data from {clientInfoUri}." +
				$" Status Code: {response.StatusCode}." +
				$" {response.ReasonPhrase}");
		}
		
		// ! is used, bc anyway if status code is 200 - profile was received
		ProfileDto profileDto = (await response.Content.ReadFromJsonAsync<ProfileDto>(cancellationToken: ct)) !;
		
		return profileDto;
	}
}