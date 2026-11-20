using HealthTracker.Api.Infrastructure;
using HealthTracker.Modules.Profiles;
using HealthTracker.SharedKernel.Modules;

var builder = WebApplication.CreateBuilder(args);

// Modules are listed explicitly: reading this line tells you what the app is made of (ADR-0002).
IModule[] modules =
[
    new ProfilesModule(),
];

builder.AddStructuredLogging();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApiDocumentation();
builder.Services.AddHealthChecks();
builder.AddPostgres();

foreach (var module in modules)
{
    module.AddServices(builder.Services, builder.Configuration);
}

var app = builder.Build();

await app.MigrateModuleDatabasesAsync();   // Development only

app.UseExceptionHandler();
app.UseStatusCodePages();   // empty 4xx/5xx responses get a ProblemDetails body too

app.MapOpenApiDocumentation();
app.MapHealthEndpoints();

var api = app.MapGroup("/api");
foreach (var module in modules)
{
    module.MapEndpoints(api);
}

await app.RunAsync();
