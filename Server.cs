using System.Net.WebSockets;
using System.Text;

class Server
{
    private static readonly List<WebSocket> Clients = new List<WebSocket>();

    public async Task handleClient(WebSocket ws)
    {   
        // Client doesnt exist
        if (!getClient().Contains(ws))
        {
            addClient(ws);
        }
        
        var buffer = new byte[1024 * 4];
        while (ws.State == WebSocketState.Open)
        {
            var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                removeClient(ws);
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                break;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine("Received: " + message);

            foreach (WebSocket client in getClient())
            {
                if (client.State == WebSocketState.Open)
                {
                    try
                    {
                        await client.SendAsync(
                            new ArraySegment<byte>(buffer, 0, result.Count),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None
                            );
                    }
                    catch
                    {
                        removeClient(client);
                    }
                }
            }
            
        }
    }

    private static void addClient(WebSocket newClient){
        lock (Clients)
        {
            Clients.Add(newClient);
        }
    }

    private static void removeClient(WebSocket deleteClient)
    {
        lock (Clients)
        {
            Clients.Remove(deleteClient);
        }
    }

    private static List<WebSocket> getClient()
    {
        lock (Clients)
        {
            return Clients.ToList();
        }
    }
}