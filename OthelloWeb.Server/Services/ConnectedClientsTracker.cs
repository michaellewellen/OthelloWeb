namespace OthelloWeb.Server.Services
{
    public class ConnectedClientsTracker
    {
        private readonly List<string> _connectionIds = new();
        private readonly object _lock = new();

        public IReadOnlyList<string> ConnectionIds
        {
            get
            {
                lock (_lock)
                {
                    return _connectionIds.ToList();
                }
            }
        }
        public void AddConnection(string connectionId)
        {
            lock (_lock)
            {
                _connectionIds.Add(connectionId);
            }
        }

        public void RemoveConnection(string connectionId)
        {
            lock (_lock)
            {
                _connectionIds.Remove(connectionId);
            }
        }
    }
}
