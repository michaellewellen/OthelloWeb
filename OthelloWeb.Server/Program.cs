using Microsoft.AspNetCore.Components.WebAssembly.Server;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.Run();
