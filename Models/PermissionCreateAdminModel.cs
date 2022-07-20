using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ERPSystemTimologio.EF;

namespace ERPSystemTimologio.Models
{
    public class PermissionCreateAdminModel : Permission
    {
        [Required, MinLength(3)]
        public new string Name { get; set; }

        public new string InvoiceAdd { get; set; }

        public new string InvoiceManage { get; set; }

        public new string InventoryManage { get; set; }

        public new string CategoryManage { get; set; }

        public new string StationManage { get; set; }

        public new string OperationManage { get; set; }

        public new string UserManage { get; set; }

        public new string PermissionManage { get; set; }
    }
}