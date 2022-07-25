using Marlin.Core;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMarlin();

var app = builder.Build();

app.UseMarlin();

app.Run();

