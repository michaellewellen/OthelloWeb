using OthelloWeb.Shared.Types;

namespace OthelloWeb.Server.Services;

public class TableManager
{
    private readonly List<Table> _tables;
    private readonly object _lock = new();

    public TableManager()
    {
        _tables = Enumerable.Range(1, 5).Select(n => new Table(n)).ToList();
    }

    public IReadOnlyList<Table> Tables
    {
        get
        {
            lock (_lock)
            {
                return _tables.ToList();
            }
        }
    }

    public TableStatus GetStatus(Table table)
    {
        if (table.Seat1ConnectionId is null && table.Seat2ConnectionId is null) return TableStatus.Empty;
        if (table.IsFull) return TableStatus.Full;
        return TableStatus.Started;
    }

    public bool SitAtTable(int tableNumber, string connectionId)
    {
        lock (_lock)
        {
            var table = _tables.FirstOrDefault(t => t.Number == tableNumber);
            if (table is null) return false;

            if (table.Seat1ConnectionId is null) { table.Seat1ConnectionId = connectionId; return true; }
            if (table.Seat2ConnectionId is null) { table.Seat2ConnectionId = connectionId; return true; }
            return false;
        }
    }

    public void LeaveTable(int tableNumber, string connectionId)
    {
        lock (_lock)
        {
            var table = _tables.FirstOrDefault(t => t.Number == tableNumber);
            if (table is null) return;

            if (table.Seat1ConnectionId == connectionId) table.Seat1ConnectionId = null;
            else if (table.Seat2ConnectionId == connectionId) table.Seat2ConnectionId = null;
        }
    }
}