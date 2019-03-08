using System;

namespace invoicing.server.Models
{
    public abstract class DataModel
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}