using System;
using System.Collections.Generic;
using System.Text;

namespace OthelloWeb.Shared.Types
{
    public class Table
    {
        public int Number { get; }
        public string? Seat1ConnectionId { get; set; }
        public string? Seat2ConnectionId { get; set; }

        public Table(int number)
        {
            Number = number;
        }

        public bool IsFull => Seat1ConnectionId != null && Seat2ConnectionId != null;
    }
}
