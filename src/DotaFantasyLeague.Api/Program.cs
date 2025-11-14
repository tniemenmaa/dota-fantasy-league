using System.Reflection;
using DotaFantasyLeague.Api.Services;
using DotaFantasyLeague.Api.Swagger;

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
builder.Services.AddHttpClient<IOpenDotaLeagueService, OpenDotaLeagueService>(client =>
{
    client.BaseAddress = new Uri("https://api.opendota.com");
});
builder.Services.AddHttpClient<IOpenDotaMatchesService, OpenDotaMatchesService>(client =>
{
    client.BaseAddress = new Uri("https://api.opendota.com");
});
builder.Services.AddHttpClient<IOpenDotaTeamsService, OpenDotaTeamsService>(client =>
{
    client.BaseAddress = new Uri("https://api.opendota.com");
});

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

app.UseAuthorization();

app.MapControllers();

app.Run();
