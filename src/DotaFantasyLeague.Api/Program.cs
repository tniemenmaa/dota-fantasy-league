using System.Reflection;
using DotaFantasyLeague.Api.Components;
using DotaFantasyLeague.Api.Services;
using DotaFantasyLeague.Api.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;

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

var app = builder.Build();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public partial class Program
{
}
