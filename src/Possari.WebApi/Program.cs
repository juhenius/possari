using Microsoft.AspNetCore.Http.Features;
using Possari.Application;
using Possari.Infrastructure;
using Possari.Presentation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(options =>
{
  options.CustomizeProblemDetails = context =>
  {
    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

    var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
  };
});

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPresentation();

var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.MapPresentation();
app.UseStatusCodePages();
app.MapDefaultEndpoints();

app.Run();
