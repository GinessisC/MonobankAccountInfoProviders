using System.Text.Json;
using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonoAccountProvider.ConsoleApp.App;
using MonoAccountProvider.ConsoleApp.App.Repositories;
using MonoAccountProvider.ConsoleApp.view;
using MonoAccountProvider.ConsoleApp.view.Interfaces;
using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.helpers;
using MonoAccountProvider.Domain.helpers.Interfaces;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Domain.UseCases;
using MonoAccountProvider.lib.src.profile.presentation.app.Interfaces;
using MonoAccountProvider.Services.Repos;
using MonoAccountProvider.Services.ResponseHandlers;
using MonoAccountProvider.Services.Services;

CoconaAppBuilder builder = CoconaApp.CreateBuilder();

builder.Services.AddMemoryCache();
builder.Services.AddTransient<CrmRequestsHandler>();

builder.Configuration.AddConfiguration(new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json")
	.AddJsonFile("userconfig.json")
	.Build());

builder.Services.AddSingleton<IUserConfigReader, FileUserConfigReader>();
builder.Services.AddSingleton<UserConfig>(s =>
{
	var reader = s.GetRequiredService<IUserConfigReader>();
	return reader.Read();
});

builder.Services.AddScoped<IAppConfigReader, AppSettingsReader>();


builder.Services.AddSingleton<JsonSerializerOptions>(_ => new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
builder.Services.AddHttpClient<CurrencyDataReceiverService>().AddHttpMessageHandler<CrmRequestsHandler>();
builder.Services.AddHttpClient<MonobankProfileService>().AddHttpMessageHandler<CrmRequestsHandler>();
builder.Services.AddHttpClient<MonobankRatesService>().AddHttpMessageHandler<CrmRequestsHandler>();

builder.Services.AddSingleton<ICurrencyInfoRepository, CurrencyInfoRepository>();
builder.Services.AddSingleton<IRatesRepository, CurrencyRatesRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();


builder.Services.AddScoped<ICurrencyOperator, CurrencyOperator>();
builder.Services.AddScoped<IAccountData, AccountData>();
builder.Services.AddScoped<IJarData, JarData>();

builder.Services.AddTransient<IExceptionPresenter, ConsoleExceptionPresenter>();
builder.Services.AddScoped<ITableRowsAdder, AccountRowsAdder>();
builder.Services.AddScoped<ITableRowsAdder, JarRowsAdder>();
builder.Services.AddScoped<ITableRowsAdder, TotalRowAdder>();

builder.Services.AddScoped<ConsolePresenter>();

CoconaApp app = builder.Build();

app.AddCommand(async (ConsolePresenter console, IExceptionPresenter exception) =>
{
	try
	{
		await console.Present();
	}
	catch (Exception e)
	{
		exception.Present(e);
	}
});

await app.RunAsync();
