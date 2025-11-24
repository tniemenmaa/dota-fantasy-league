using System.Reflection;
using DotaFantasyLeague.Api.Components;
using DotaFantasyLeague.Api.Configuration;
using DotaFantasyLeague.Api.Data;
using DotaFantasyLeague.Api.Services;
using DotaFantasyLeague.Api.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    options.OperationFilter<DefaultLeagueIdOperationFilter>();
});
builder.Services.AddHttpClient<IOpenDotaService, OpenDotaService>(client =>
{
    client.BaseAddress = new Uri("https://api.opendota.com");
});

builder.Services.AddHttpClient<IStratzGraphQlService, StratzGraphQlService>(client =>
{
    client.BaseAddress = new Uri("https://api.stratz.com/graphql");
    client.DefaultRequestHeaders.Add("User-Agent", "STRATZ_API");
    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + builder.Configuration["Stratz:ApiToken"]);
});

builder.Services.AddHttpClient("SteamOpenId", client =>
{
    client.BaseAddress = new Uri("https://steamcommunity.com");
});

builder.Services.AddHttpClient("SteamWebApi", client =>
{
    client.BaseAddress = new Uri("https://api.steampowered.com");
});

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/signin/steam";
        options.LogoutPath = "/auth/signout";
    });

builder.Services.AddAuthorization();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

var healthChecks = builder.Services.AddHealthChecks();
healthChecks.AddCheck("self", () => HealthCheckResult.Healthy());

builder.Services.Configure<CosmosDbOptions>(builder.Configuration.GetSection(CosmosDbOptions.SectionName));

var cosmosDbOptions = builder.Configuration.GetSection(CosmosDbOptions.SectionName).Get<CosmosDbOptions>();
if (cosmosDbOptions?.IsConfigured == true)
{
    builder.Services.AddDbContext<DotaFantasyLeagueDbContext>(options =>
    {
        options.UseCosmos(cosmosDbOptions.AccountEndpoint!, cosmosDbOptions.AccountKey!, cosmosDbOptions.DatabaseName!);
    });

    healthChecks.AddDbContextCheck<DotaFantasyLeagueDbContext>("cosmos-db");
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<DotaFantasyLeagueDbContext>();
    if (dbContext is not null)
    {
        await dbContext.Database.EnsureCreatedAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();

public partial class Program
{
}
