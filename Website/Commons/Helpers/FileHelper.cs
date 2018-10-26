using Models.Class;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Configuration;
using CommonsConst;
using Models.Class.FileUpload;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Hosting;
using RestSharp;

namespace Commons
{
    public class FileHelper
    {
        private static readonly string EncryptionSalt = "d41d8cd98f00b204e9800998ecf8427e";
        public static List<string> AcceptedVideoTypes = new List<string> { ".mp4", ".m4v", ".mov", ".mkv", ".avi" };
        public static List<string> AcceptedImageTypes = new List<string> { ".bmp", ".jpeg", ".jpg", ".png", ".gif", ".pdf" };

        /// <summary>
        /// Deletes the specified document.
        /// </summary>
        /// <returns><c>true</c>, if document was deleted, <c>false</c> otherwise.</returns>
        /// <param name="cheminDocument">Chemin document.</param>
        public static bool DeleteDocument(string FilePath)
        {
            bool result = true;
            try
            {
                if (!String.IsNullOrEmpty(FilePath))
                {
                    var TabUrl = FilePath.Split(new[] { "/Ressources" }, StringSplitOptions.None);
                    if (TabUrl.Length > 1)
                    {
                        FilePath = "~/Ressources" + TabUrl[1];
                        string FileName = Path.GetFileName(FilePath);
                        string urlRelative = FilePath.Replace(FileName, "");
                        string urlAbsolue = GetStorageRoot(urlRelative) + FileName;
                        System.IO.File.Delete(urlAbsolue);
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FilePath = " + FilePath);
            }
            return result;
        }


        public static Stream GetStreamFromPath(string path)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(path))
                {
                    if (path.ToLower().Contains("http://") || path.ToLower().Contains("https://"))
                    {
                        return GetStreamFromUrl(path);
                    }
                    else
                    {
                        return File.OpenRead(GetStorageRoot(path));
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,"path = " + path);
            }
            return null;
        }

        private static Stream GetStreamFromUrl(string url)
        {
            try
            {
                byte[] file = null;

                using (var wc = new System.Net.WebClient())
                    file = wc.DownloadData(url);

                return new MemoryStream(file);
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "url = " + url);
            }
            return null;
        }

        public static bool FileExists(string path)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(path))
                {
                    if (path.ToLower().Contains("http://") || path.ToLower().Contains("https://"))
                    {
                        var client = new RestClient(path);
                        var request = new RestRequest(Method.HEAD);
                        request.Timeout = 5000;
                        var response = client.Execute(request);
                        result = response.StatusCode == HttpStatusCode.OK;
                    }
                    else
                    {
                        result = File.Exists(GetStorageRoot(path));
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "path = " + path);
            }
            return result;
        }


        /// <summary>
        /// Check if the file is a video
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsValidVideo(HttpPostedFileBase file)
        {
            bool result = false;
            try
            {

                if (file == null || file.ContentLength == 0)
                {
                    result = false;
                }
                else
                {
                    string fileName = Utils.RemoveUnsafeCharacters(file.FileName);
                    string ext = Path.GetExtension(fileName).ToLower();

                    if (!AcceptedVideoTypes.Contains(ext))
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                string FileName = "Unknown";
                if (file != null)
                    FileName = file.FileName;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileName = " + FileName);
            }
            return result;
        }


        /// <summary>
        /// Save  a given file from an url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Purpose"></param>
        /// <param name="Extension"></param>
        /// <returns></returns>
        public static string SaveFileFromWeb(string url, string Purpose, string Extension=null)
        {
            string result = null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                WebClient myWebClient = new WebClient();
                byte[] data = myWebClient.DownloadData(url);

                if(Extension == null)
                {
                    Extension = Path.GetExtension(url);
                }

                string FileName = GetFileName(Purpose, Extension);
                string DiskPath = GetStorageRoot(Const.BasePathUploadDecrypted) + "\\" + FileName;
                File.WriteAllBytes(DiskPath, data);
                result = Const.BasePathUploadDecrypted + "/" + FileName;
            }
            catch (Exception e)
            {
                result = url;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "url = " + url + " and Purpose = " + Purpose);
            }
            return result;
        }


        /// <summary>
        /// Save and ecrypt a given file from an url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Purpose"></param>
        /// <param name="Extension"></param>
        /// <returns></returns>
        public static string SaveAndEncryptFileFromWeb(string url, string Purpose, string Extension)
        {
            string result = null;
            try
            {
                WebClient myWebClient = new WebClient();
                byte[] data = myWebClient.DownloadData(url);

                string FileName = GetFileName(Purpose, Extension);
                string DiskPath = GetStorageRoot(Const.BasePathUploadEncrypted) + "\\" + FileName;

                if (EncryptWriteBytes(DiskPath, data))
                {
                    result = Const.BasePathUploadEncrypted + "/" + FileName;
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "url = " + url + " and Purpose = " + Purpose);
            }
            return result;
        }


        /// <summary>
        /// Check if the file is an image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsValidImage(HttpPostedFileBase file)
        {
            bool result = false;
            try
            {

                if (file == null || file.ContentLength == 0)
                {
                    result = false;
                }
                else
                {
                    string fileName = Utils.RemoveUnsafeCharacters(file.FileName);
                    string ext = Path.GetExtension(fileName).ToLower();

                    if (!AcceptedImageTypes.Contains(ext))
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                string FileName = "Unknown";
                if (file != null)
                    FileName = file.FileName;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileName = " + FileName);
            }
            return result;
        }


        /// <summary>
        /// Get the root path on the server
        /// </summary>
        /// <returns></returns>
        public static string GetRootPathDefault()
        {
            try
            {
                var dirPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                return dirPath;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return "";
        }


        /// <summary>
        /// Retourne l'adresse absolue d'un répertoire sur le serveur
        /// </summary>
        /// <returns>The storage root.</returns>
        /// <param name="url">URL.</param>
        public static string GetStorageRoot(string url)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(url)  && !(url.Contains("D:") || url.Contains("C:") || url.Contains("https://") || url.Contains("http://")))
                {
                    string StorageRoot = GetRootPathDefault() + url.Replace("~/", "").Replace("/", @"\");
                    return StorageRoot;
                }
                else
                {
                    return url;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "url = " + url);
            }
            return "";
        }

        /// <summary>
        /// Cjeck if a file is a pdf
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsPdfFile(HttpPostedFileBase file)
        {
            bool result = false;
            try
            {
                if (file != null)
                {
                    string fileName = file.FileName.Replace("<", "").Replace(">", "").Replace(":", "").Replace("\"", "").Replace("/", "").Replace("\\", "").Replace("|", "").Replace("?", "").Replace("*", "");
                    string fileExt = Path.GetExtension(fileName);

                    if (fileExt.ToLower() == ".pdf")
                    {
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                string FileName = "Unknown";
                if (file != null)
                    FileName = file.FileName;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileName = " + FileName);
            }
            return result;
        }

        /// <summary>
        /// Encrypt the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        private static bool EncryptWriteBytes(string path, byte[] fileBytes)
        {
            bool result = false;
            try
            {
                var encryptedBytes = RijndaelHelper.EncryptBytes(fileBytes, ConfigurationManager.AppSettings["FileEncryptPassPhrase"], EncryptionSalt);
                File.WriteAllBytes(path, encryptedBytes);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "path = " + path);
            }
            return result;
        }

        /// <summary>
        /// Save am image captured with a webcam
        /// </summary>
        /// <param name="Stream"></param>
        /// <param name="Purpose"></param>
        /// <returns></returns>
        public static string WebcamCapture(Stream Stream, string Purpose)
        {
            string returnPath = null;
            try
            {
                string dump = "";

                using (var reader = new StreamReader(Stream))
                    dump = reader.ReadToEnd();
                if (!String.IsNullOrWhiteSpace(dump))
                {
                    string ext = ".jpeg";
                    string fileName = GetFileName(Purpose, ext);
                    var path = FileHelper.GetStorageRoot(Const.BasePathUploadDecrypted) + "/" + fileName;
                    File.WriteAllBytes(path, Commons.FileHelper.String_To_Bytes2(dump));
                    returnPath = Const.BasePathUploadDecrypted + "/" + fileName;
                }
            }
            catch (Exception e)
            {
                returnPath = null;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return returnPath;
        }

        /// <summary>
        /// Return the name of the file
        /// </summary>
        /// <param name="Purpose"></param>
        /// <param name="Extension"></param>
        /// <returns></returns>
        public static string GetFileName(string Purpose, string Extension)
        {
            string result = "";
            try
            {
                Random rnd = new Random();
                string strRandom = rnd.Next(1, 10000).ToString();
                result = DateTime.UtcNow.ToString("yyyyMMddhhmmss_ffffff") + strRandom + "_" + Purpose + Extension;
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Purpose = " + Purpose + " and Extension = " + Extension);
            }

            return result;
        }

        /// <summary>
        /// Return an array of bytes in order to download the file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] GetFileToDownLoad(string path)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(path))
                {
                    var localFilePath =GetStorageRoot(path);
                    if (FileHelper.FileExists(localFilePath))
                    {
                        var FileBytes = File.ReadAllBytes(localFilePath);
                        // If the file is in the upload folder, it needs to be decrypted
                        if (path.ToLower().Contains("/encrypted/") || path.ToLower().Contains("\\encrypted\\"))
                        {
                            var decyptedFileBytes = RijndaelHelper.DecryptBytes(FileBytes, ConfigurationManager.AppSettings["FileEncryptPassPhrase"], EncryptionSalt);
                            return decyptedFileBytes;
                        }
                        else
                        {
                            return FileBytes;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "path = " + path);
            }
            return null;
        }

        /// <summary>
        /// Get an image from a 64 base
        /// </summary>
        /// <param name="Src64base"></param>
        /// <returns></returns>
        public static Image GetImageFrom64Base(string Src64base)
        {
            try
            {
                Image image;
                if (Src64base.Contains("data:image"))
                {
                    foreach (var ext in AcceptedImageTypes)
                    {
                        Src64base = Src64base.Replace("data:image/" + ext.Replace(".", "").ToLower() + ";base64,", "");
                    }

                    byte[] bytes = Convert.FromBase64String(Src64base);


                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }
                }
                else
                {
                    image = image = Image.FromFile(Src64base);
                }
                return image;
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }

            return null;
        }

        /// <summary>
        /// get the path of the decrypted file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="IsUserPicture"></param>
        /// <returns></returns>
        public static string GetDecryptedFilePath(string path, bool IsUserPicture = false, bool IsThumbnail = false)
        {
            string fileSrc = CommonsConst.DefaultImage.Default;
            try
            {
                if (!String.IsNullOrWhiteSpace(path))
                {
                    var localFilePath =  GetStorageRoot(path);
                    if (FileHelper.FileExists(localFilePath))
                    {
                        // If the file is in the upload folder, it needs to be decrypted
                        if (path.Contains(CommonsConst.Const.BasePathUploadEncrypted))
                        {
                            var encyptedFileBytes = File.ReadAllBytes(localFilePath);
                            var decyptedFileBytes = RijndaelHelper.DecryptBytes(encyptedFileBytes, ConfigurationManager.AppSettings["FileEncryptPassPhrase"], EncryptionSalt);
                            fileSrc = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(decyptedFileBytes));
                        }
                        else
                        {
                            fileSrc = path.Replace("~", "");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                fileSrc = CommonsConst.DefaultImage.Default.Replace("~", "");
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "path = " + path + " and IsUserPicture = " + IsUserPicture);
            }

            if (IsUserPicture && fileSrc == CommonsConst.DefaultImage.Default.Replace("~", ""))
            {
                if (IsThumbnail)
                {
                    fileSrc = CommonsConst.DefaultImage.DefaultThumbnailUser.Replace("~", "");
                }
                else
                {
                    fileSrc = CommonsConst.DefaultImage.DefaultImageUser.Replace("~", "");
                }
            }

            return fileSrc;
        }




        /// <summary>
        /// Transform a string to a bytes array
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static byte[] String_To_Bytes2(string strInput)
        {
            byte[] bytes = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(strInput))
                {

                    int numBytes = (strInput.Length) / 2;
                    bytes = new byte[numBytes];

                    for (int x = 0; x < numBytes; ++x)
                    {
                        bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.BaseType, "strInput = " + strInput);
            }
            return bytes;
        }

 


        public static string UploadDecryptedFile(HttpPostedFileBase file, string filename)
        {
            string retour = "";
            try
            {
                string uploadPath = CommonsConst.Const.BasePathUploadDecrypted+"/";
                string path = HostingEnvironment.MapPath("~" + uploadPath + filename);
                file.SaveAs(path);

                retour = uploadPath + filename;
            }
            catch (Exception e)
            {
                retour = "KO";
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "filename = " + filename);
            }

            return retour;
        }


        /// <summary>
        ///  Upload a file on the server
        /// </summary>
        /// <param name="FileUpload"></param>
        /// <returns></returns>
        public static string UploadFile(FileUpload FileUpload)
        {
            string retour = "";
            try
            {
                if (FileUpload == null)
                {
                    retour = "KO";
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(FileUpload.UploadName))
                        FileUpload.UploadName = FileUpload.File.FileName;

                    FileUpload.UploadName = FileUpload.UploadName.ToLower();

                    if (FileUpload.IsImage && IsPdfFile(FileUpload.File))
                    {
                        FileUpload.FileBytes = ImageHelper.ConvertPdfToPngBytes(FileUpload);
                        FileUpload.UploadName = FileUpload.UploadName.Replace(".pdf", ".png");
                    }
                    else
                    {
                        using (var ms = new MemoryStream())
                        {
                            FileUpload.File.InputStream.Position = 0;
                            FileUpload.File.InputStream.CopyTo(ms);
                            FileUpload.FileBytes = ms.ToArray();
                        }
                    }


                    string uploadPath = CommonsConst.Const.BasePathUploadEncrypted;
                    if (!FileUpload.EncryptFile)
                    {
                        uploadPath = CommonsConst.Const.BasePathUploadDecrypted;
                    }

                    string DiskPath = GetStorageRoot(uploadPath) + "/" + FileUpload.UploadName;
                    if (FileUpload.EncryptFile)
                    {
                        if (!EncryptWriteBytes(DiskPath, FileUpload.FileBytes))
                            retour = "KO";
                    }
                    else
                    {
                        File.WriteAllBytes(DiskPath, FileUpload.FileBytes);
                    }

                    if (retour != "KO")
                        retour = uploadPath + "/" + FileUpload.UploadName;
                }
            }
            catch (Exception e)
            {
                retour = "KO";
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileNameOnServer = " + FileUpload.UploadName);
            }

            return retour;
        }
    }
}
