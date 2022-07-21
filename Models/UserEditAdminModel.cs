using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ERPSystemTimologio.EF;

namespace ERPSystemTimologio.Models
{
    public class UserEditAdminModel : User
    {
        [Required]
        [RegularExpression(@"^[10]+$", ErrorMessage = "The field must 0 or 1")]
        public new int Verified { get; set; }
        [Required]
        [MinLength(3)]
        public new string Name { get; set; }

        //[Required]
        //[MinLength(3)]
        //[UniqueUsernameValidation]
        //public new string Username { get; set; }

        //[Required]
        //[EmailAddress]
        //[UniqueEmailValidation]
        //public new string Email { get; set; }

        [Required]
        [Range(0, 500000)]
        public new double? Salary { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public new DateTime? HireDate { get; set; }

        [Required]
        [Range(1, 4)]
        public new int Type { get; set; }

        [Range(0, int.MaxValue)]
        public new int? RegionId { get; set; }

        [Range(0, int.MaxValue)]
        public new int? BranchId { get; set; }

        public string LocalAddress { get; set; }

        public string PoliceStation { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public List<int> PermissionIds { get; set; }
    }
}