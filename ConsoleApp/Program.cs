using System.Text.Json;
using Application.Helpers;
using Application.Repositories;
using Application.Services;
using Cocona;
using Cocona.Builder;
using ConsoleApp.App;
using ConsoleApp.App.Interfaces;
using ConsoleApp.View;
using ConsoleApp.View.Interfaces;
using Domain.Entities.DataSources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonoAccountProvider.Domain.Helpers.Interfaces;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Repositories;
using MonoAccountProvider.Services.RequestHandlers;
using MonoAccountProvider.Services.Services;

CoconaAppBuilder builder = CoconaApp.CreateBuilder();

builder.Services.AddMemoryCache(options => options.SizeLimit = 1024);
builder.Services.AddSingleton<CancellationTokenSource>();

builder.Services.AddTransient<CurrencyInfoRequestHandler>();
builder.Services.AddTransient<CurrencyRatesRequestHandler>();
builder.Services.AddTransient<MonobankProfileRequestHandler>();

builder.Configuration
	.AddJsonFile("appsettings.json")
	.AddJsonFile("userconfig.json");

builder.Services.Configure<UserCfgOptions>(builder.Configuration.GetSection(nameof(UserCfgOptions)));

builder.Services.Configure<CurrencyInfoSourceOptions>(
	builder.Configuration.GetSection(nameof(CurrencyInfoSourceOptions)));

builder.Services.Configure<ProfileSourceOptions>(builder.Configuration.GetSection(nameof(ProfileSourceOptions)));
builder.Services.Configure<RatesSourceOptions>(builder.Configuration.GetSection(nameof(RatesSourceOptions)));

builder.Services.AddKeyedSingleton(JsonSerializationOptionNames.PropertyNameCaseInsensitive, new JsonSerializerOptions
{
	PropertyNameCaseInsensitive = true
});

// Do not create new httpClient in parameters. It leads to overflow of clients
builder.Services.AddHttpClient<CurrencyInfoReceiverService>().AddHttpMessageHandler<CurrencyInfoRequestHandler>();
builder.Services.AddHttpClient<MonobankProfileService>().AddHttpMessageHandler<MonobankProfileRequestHandler>();
builder.Services.AddHttpClient<CurrencyRatesService>().AddHttpMessageHandler<CurrencyRatesRequestHandler>();

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

app.AddCommand(async (ConsolePresenter console,
	IExceptionPresenter exception,
	CancellationTokenSource cts) =>
{
	CancellationToken ct = cts.Token;

	try
	{
		await console.Present(ct);
	}
	catch (Exception e)
	{
		exception.Present(e);
	}
});

await app.RunAsync();