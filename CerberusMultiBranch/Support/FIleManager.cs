using System;
using System.Collections.Generic;
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

        public static string GetExtension(string base64)
        {
            var code = base64.Substring(0, 5).ToUpper();

            switch (code)
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "AAABA":
                    return "ico";
                case "JVBER":
                    return "pdf";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "MQOWM":
                    return "txt";
                default:
                    return string.Empty;

            }

        }


        public static string SaveFile(string base64, string path, string fileName, bool deleteOldCopy = true)
        {
            try
            {
                var spl = new List<string> { "base64," };
                base64 = base64.Split(spl.ToArray(), StringSplitOptions.RemoveEmptyEntries)[1];

                var fileInBytes = Convert.FromBase64String(base64);

                var name      = fileName;
                var size      = fileInBytes.Length;
                var extension = GetExtension(base64);
                var physicalPath = HttpContext.Current.Server.MapPath(path);

                fileName = fileName + "." + extension;

                var fullVirtualPath     = path+"/"+ fileName;

                var fullPhysicalPath    = Path.Combine(physicalPath, fileName); 

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                if (File.Exists(physicalPath))
                {
                    if (deleteOldCopy)
                        File.Delete(fullPhysicalPath);
                }

                File.WriteAllBytes(fullPhysicalPath, fileInBytes);

                return fullVirtualPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool DropFile(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
            
            return true;
        }

        public static string SaveImage(HttpPostedFileBase image, string parentId, ImageType type)
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

                if (type == ImageType.UserProfile && Directory.Exists(serverPath))
                    Directory.Delete(serverPath, true);

                Directory.CreateDirectory(serverPath);

                var fullRealPath = Path.Combine(serverPath, fileName);
                var fullVirtualPath = serverUrl + "/" + fileName;
                
                image.SaveAs(fullRealPath);

                return fullVirtualPath;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        public static string SaveImage(HttpPostedFile image, string parentId, ImageType type)
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

                if (type == ImageType.UserProfile && Directory.Exists(serverPath))
                    Directory.Delete(serverPath,true);

                Directory.CreateDirectory(serverPath);

                var fullRealPath = Path.Combine(serverPath, fileName);
                var fullVirtualPath = serverUrl + "/" + fileName;

                image.SaveAs(fullRealPath);

                return fullVirtualPath;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
    }
}