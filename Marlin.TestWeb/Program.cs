using Marlin.Core;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapWhen(context => context.Request.Path.ToString().StartsWith("/"), appBranch => {
    appBranch.UseMiddleware<MarlinMiddleware>();
});

app.MapGet("/", () => "Hello World!");

app.Run();