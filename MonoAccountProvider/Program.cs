using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.DependencyInjection;
using MonoAccountProvider.lib.src.profile.data.datasource;
using MonoAccountProvider.lib.src.profile.data.repos;
using MonoAccountProvider.lib.src.profile.domain.cache;
using MonoAccountProvider.lib.src.profile.domain.cache.Interfaces;
using MonoAccountProvider.lib.src.profile.domain.helpers;
using MonoAccountProvider.lib.src.profile.domain.helpers.Interfaces;
using MonoAccountProvider.lib.src.profile.domain.repos;
using MonoAccountProvider.lib.src.profile.domain.usecases;
using MonoAccountProvider.lib.src.profile.presentation.app;
using MonoAccountProvider.lib.src.profile.presentation.app.Interfaces;
using MonoAccountProvider.lib.src.profile.presentation.app.repos;
using MonoAccountProvider.lib.src.profile.presentation.view;

CoconaAppBuilder builder = CoconaApp.CreateBuilder();

builder.Services.AddHttpClient<CurrencyDataReceiverService>();
builder.Services.AddHttpClient<MonobankProfileService>();
builder.Services.AddHttpClient<MonobankRatesService>();

builder.Services.AddSingleton<ICurrencyDataRepo, CurrencyDataRepo>();
builder.Services.AddSingleton<IRatesRepo, CurrencyRatesRepo>();
builder.Services.AddScoped<IProfileRepo, ProfileRepo>();

builder.Services.AddScoped<IConfigReader, FileConfigReader>();

await RegisterCacheServices();

builder.Services.AddScoped<ICurrencyHelper, CurrencyHelper>();
builder.Services.AddScoped<IAccountData, AccountData>();
builder.Services.AddScoped<IJarData, JarData>();

builder.Services.AddTransient<IExceptionPresenter, ConsoleExceptionPresenter>();
builder.Services.AddScoped<ConsolePresenter>();

CoconaApp app = builder.Build();

app.AddCommand((ConsolePresenter console, IExceptionPresenter exception) =>
{
	try
	{
		console.Present();
	}
	catch (Exception e)
	{
		exception.Present(e);
	}
});

await app.RunAsync();

async Task RegisterCacheServices()
{
	ServiceProvider provider = builder.Services.BuildServiceProvider();

	IProfileRepo profileRepo = provider.GetRequiredService<IProfileRepo>();
	IConfigReader configReader = provider.GetRequiredService<IConfigReader>();
	IRatesRepo ratesRepo = provider.GetRequiredService<IRatesRepo>();
	ICurrencyDataRepo currencyRepo = provider.GetRequiredService<ICurrencyDataRepo>();

	CurrencyInfoCache currencyDataRepo = await CurrencyInfoCache.Create(ratesRepo, currencyRepo);
	UserCache userCache = await UserCache.CreateAsync(profileRepo, configReader);

	builder.Services.AddScoped<IUserCache, UserCache>(_ => userCache);
	builder.Services.AddSingleton<ICurrencyInfoCache, CurrencyInfoCache>(_ => currencyDataRepo);
}