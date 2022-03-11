using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.Models
{
    public class SysRole
    {
        [Key] 
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public DateTime DateCreated { get; set; }

        // [ForeignKey("UserID")]
       // public int CreatedBy { get; set; }
        public bool IsActive { get; set; }

        //[InverseProperty("Role")]
        //public IEnumerable<SysUsers> Users { get; set; }

    }
}
