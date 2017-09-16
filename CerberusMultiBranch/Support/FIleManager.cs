using System;
using System.IO;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Support
{
    public enum ImageType
    {
        Products,
        UserProfile,
        EmployeeCard
    }
    public class FileManager
    {
        public static string SaveImage(HttpPostedFileBase image, int parentId, ImageType type)
        {
            try
            {
                var fileName = Path.GetFileName(image.FileName); //getting only file name(ex-ganesh.jpg)  
                var extension = Path.GetExtension(image.FileName); //getting the extension(ex-.jpg)
                
                string serverUrl = string.Empty;

                switch (type)
                {
                    case ImageType.UserProfile:
                        serverUrl = Cons.UserProfilePath + "/" + parentId.ToString();
                        break;
                    case ImageType.EmployeeCard:
                        serverUrl = Cons.EmployeeProfilePath + "/" + parentId.ToString();
                        break;
                    case ImageType.Products:
                        serverUrl = Cons.ProductImagesPath + "/" + parentId.ToString();
                        break;
                }

                var serverPath = HttpContext.Current.Server.MapPath(serverUrl);

                Directory.CreateDirectory(serverPath);

                var fullRealPath = Path.Combine(serverPath, fileName);
                var fullVirtualPath = serverUrl + "/" + fileName;

                image.SaveAs(fullRealPath);

                return fullVirtualPath;
            }
            catch (System.Exception ex)
            {
                return string.Empty;
            }
        }
    }
}