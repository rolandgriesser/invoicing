using System;
using System.ComponentModel.DataAnnotations;

namespace invoicing.server.Models
{
    public class Organization : DataModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}