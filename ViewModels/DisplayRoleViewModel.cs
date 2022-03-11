using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.ViewModels
{
    public class DisplayRoleViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
       
    }



    public class HoldDisplayRoleViewModel
    {
        public IEnumerable<DisplayRoleViewModel> HoldAllRoles { get; set; }
      
    }


    public class SystemUserViewModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public DateTime DateCreated { get; set; } 
        public int RoleID { get; set; }
        public string Role { get; set; } 
        public string CompanyName { get; set; } 
    }


    public class HoldSystemUserViewModel
    {
        public IEnumerable<SystemUserViewModel> HoldAllSystemUsers { get; set; }
        //public Pager Pager { get; set; }
    }


}
