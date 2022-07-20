using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ERPSystemTimologio.Models
{
    public class ProfilePicture
    {
        public string ErrorMessage { get; set; }
        public decimal Filesize { get; set; }
        public string Upload(HttpPostedFileBase file, string username)
        {
            try
            {
                var supportFileType = new[] { "jpg", "jpeg", "png", "gif" };
                var getFileExtension = Path.GetExtension(file.FileName).Substring(1);

                if (!supportFileType.Contains(getFileExtension))
                {
                    ErrorMessage = "File must be jpg/jpeg/png/gif format";
                    return ErrorMessage;
                }
                else if (file.ContentLength > (Filesize * 1024))
                {
                    ErrorMessage = "File size must be less than " + Filesize + "KB";
                    return ErrorMessage;
                }

                string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/images/avatar"), username + "." + getFileExtension);
                file.SaveAs(_path);

                return null;
            }
            catch (Exception)
            {
                ErrorMessage = "Upload Container Should Not Be Empty";
                return ErrorMessage;
            }
        }
    }
}