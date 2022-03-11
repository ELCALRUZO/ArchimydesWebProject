using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.ViewModels
{
    public class APIService
    { 
            public string Url { get; set; } 
            public bool IsEnabled { get; set; }
       
    }


    public class APIServiceReponse
    {
        public int Status { get; set; }
        public string Message { get; set; }

    }

    public class UserResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string DateCreated { get; set; }
        public string CompanyName { get; set; }

    }



}
