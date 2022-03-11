using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.Models
{
    public class ApplicationUser : IdentityUser
    { 
        [ForeignKey("UserID")]
        public int UserID { get; set; }
    }
}
