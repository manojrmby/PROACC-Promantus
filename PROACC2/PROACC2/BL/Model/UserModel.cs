using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class UserModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public System.Guid UserId { get; set; }

        public string ProjectManagerName { get; set; }
        //[Required(ErrorMessage = "Password is required")]
        //[DataType(DataType.Password)]
        public System.Guid ProjectManagerId { get; set; }


        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LoginId { get; set; }
        public int RoleID { get; set; }
        public int UserTypeID { get; set; }
        public Nullable<System.Guid> Customer_Id { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
        public string Countrycode { get; set; }
        public string DialCode { get; set; }
        public string RoleName { get; set; }
        public string UserType { get; set; }
        public string Customer_Name { get; set; }
        public string Status { get; set; }
        public byte[] TS { get; set; }

    }
}