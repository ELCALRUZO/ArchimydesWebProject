using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.Models
{
    public class SysUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public DateTime DateCreated { get; set; }

        public int RoleID { get; set; }
        //[ForeignKey("RoleID")]
        //public SysRole Role { get; set; }




    }
}
