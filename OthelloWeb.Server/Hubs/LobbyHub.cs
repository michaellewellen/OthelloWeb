namespace OthelloWeb.Server.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using OthelloWeb.Server.Services;

    public class LobbyHub : Hub
    {
        private readonly ConnectedClientsTracker _tracker;
        private readonly TableManager _tableManager;
        public LobbyHub(ConnectedClientsTracker tracker, TableManager tableManager)
        {
            _tracker = tracker;
            _tableManager = tableManager;
        }

        public override async Task OnConnectedAsync()
        {
            _tracker.AddConnection(Context.ConnectionId);
            Console.WriteLine($"About to broadcast to {_tracker.ConnectionIds.Count} clients");

            Console.WriteLine($"Client connected: {Context.ConnectionId}, total: {_tracker.ConnectionIds.Count}");
            await Clients.All.SendAsync("UpdateClientList", _tracker.ConnectionIds);
            Console.WriteLine("Broadcast completed successfully");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _tracker.RemoveConnection(Context.ConnectionId);
            _tableManager.LeaveTable(FindTableFor(Context.ConnectionId), Context.ConnectionId);
            await Clients.All.SendAsync("UpdateClientList", _tracker.ConnectionIds);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinTable(int tableNumber)
        {
            var success = _tableManager.SitAtTable(tableNumber, Context.ConnectionId); // connectionID is the client calling in 
            if (success)
            {
                await Clients.All.SendAsync("UpdateTableList", _tableManager.Tables);
            }
        }

        public async Task LeaveTable(int tableNumber)
        {
            _tableManager.LeaveTable(tableNumber, Context.ConnectionId);
            await Clients.All.SendAsync("UpdateTableList", _tableManager.Tables);
        }

        private int FindTableFor(string connectionId)
        {
            var table = _tableManager.Tables.FirstOrDefault(t => 
                t.Seat1ConnectionId == connectionId || t.Seat2ConnectionId == connectionId);
            return table?.Number ?? -1;
        }
    }
}
