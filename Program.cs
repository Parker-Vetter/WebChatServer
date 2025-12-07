using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Server server = new Server();
Menu menu = new Menu(server);
menu.Run();

// Enable WebSockets
app.UseWebSockets();

// WebSocket endpoint
app.Map("/ws", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = 400;
        return;
    }

    WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
    await server.handleClient(ws);
});

app.Run("http://localhost:1050");
