using GrpcWindowsService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting.WindowsServices;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // If running as a Windows Service, pre-set ContentRootPath
    // to avoid UseWindowsService() changing it later.
    ContentRootPath = WindowsServiceHelpers.IsWindowsService()
        ? AppContext.BaseDirectory
        : default
});

if (WindowsServiceHelpers.IsWindowsService())
{
    // Run the Kestrel server on HTTP/2 over HTTP only
    builder.WebHost.ConfigureKestrel(o =>
    {
        o.ListenLocalhost(5099, l =>
        {
            l.Protocols = HttpProtocols.Http2;
        });
    });
}

builder.Host.UseWindowsService();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();