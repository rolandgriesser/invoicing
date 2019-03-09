using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace invoicing.server.Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

    }
}
