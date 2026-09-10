using Microsoft.AspNetCore.Components.WebAssembly.Server;
using OthelloWeb.Server.Services; /// folder with services in it...
using OthelloWeb.Server.Hubs; /// folder with hubs in it... 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR(); // adding the service 
builder.Services.AddSingleton<ConnectedClientsTracker>(); // adding the connected clients tracker as a singleton service
builder.Services.AddSingleton<TableManager>(); // adding the table manager as a singleton service

var app = builder.Build();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapHub<LobbyHub>("/lobbyhub"); // mapping the hub to a route... 'lobbyhub' is arbitrary, but it should match the route used in the client to connect to the hub.
app.MapFallbackToFile("index.html");

app.Run();
