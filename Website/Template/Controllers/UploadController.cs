using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Models;
using Models.ViewModels;
using Service;
using Commons;
using i18n;
using System.IO;
using Models.Class;
using Models.Class.FileUpload;
using Service.UserArea.Interface;

namespace Website.Controllers
{
    public class UploadController : BaseController
    {

        public UploadController(
            IUserService userService
            ) : base(userService)
        {

        }

        /// <summary>
        ///  Get the picture
        /// </summary>
        /// <param name="Purpose"></param>
        /// <param name="WebcamSessionId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWebcamCapture(string Purpose, string WebcamSessionId)
        {
            bool _success = false;
            string _Error = "";
            string _PathFile = "";
            string _PathFilePreview = "";
            try
            {

                if (Session[CommonsConst.Const.WebcamCaptureSession + Purpose+ WebcamSessionId] != null)
                {
                    _PathFile = Session[CommonsConst.Const.WebcamCaptureSession + Purpose + WebcamSessionId].ToString();

                    _PathFilePreview = FileHelper.GetDecryptedFilePath(_PathFile, true);
                    _success = true;
                }


            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.BaseType, "Purpose = " + Purpose);
            }
            return Json(new { Result = _success, PathFile = _PathFile, Error = _Error, PathFilePreview = _PathFilePreview });
        }

        /// <summary>
        /// Save the picture
        /// </summary>
        /// <param name="Purpose"></param>
        /// <param name="WebcamSessionId"></param>
        /// <returns></returns>
        public ActionResult SaveWebcamCapture(string Purpose, string WebcamSessionId)
        {
            bool _success = false;
            string _Error = "";
            string _PathFile = "";
            string _PathFilePreview = "";
            try
            {

                Stream stream = Request?.InputStream;
                if (stream != null)
                {
                    _PathFile = FileHelper.WebcamCapture(stream, Purpose);
                    if (!String.IsNullOrWhiteSpace(_PathFile))
                    {
                        _success = true;
                        Session[CommonsConst.Const.WebcamCaptureSession + Purpose + WebcamSessionId] = _PathFile;
                    }
                }

            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.BaseType, "Purpose = " + Purpose);
            }
            return Json(new { Result = _success, PathFile = _PathFile, Error = _Error, PathFilePreview = _PathFilePreview });
        }

        /// <summary>
        ///  Upload a decypted document
        /// </summary>
        /// <param name="Purpose"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadDecryptedDocument(string Purpose)
        {
            bool _success = false;
            string _PathFile = "";
            try
            {

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];


                        var fileName = Path.GetFileName(file.FileName);


                            string ext = Path.GetExtension(fileName);
                            fileName = FileHelper.GetFileName(Purpose, ext);

                            FileUpload newFile = new FileUpload()
                            {
                                File = file,
                                UploadName = fileName,
                                IsImage = false,
                                EncryptFile = false
                            };

                            string retour = FileHelper.UploadFile(newFile);
                            if (retour != "KO")
                            {
                                _success = true;
                                _PathFile = retour;
                            }

                    }
            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Json(new { Result = _success, PathFile = _PathFile});
        }

        /// <summary>
        /// Generic picture upload
        /// </summary>
        /// <param name="Purpose"></param>
        /// <param name="EncryptFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadPicture(string Purpose, bool EncryptFile = false, int? SizeLimit = CommonsConst.FileSize.LightPicture)
        {
            bool _success = false;
            string _Error = "";
            string _PathFile = "";
            string _PathFilePreview = "";
            try
            {
                if(SizeLimit==null)
                {
                    SizeLimit = CommonsConst.FileSize.LightPicture;
                }


                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file.ContentLength <= SizeLimit) 
                    {

                        var fileName = Path.GetFileName(file.FileName);

                        if (FileHelper.IsValidImage(file))
                        {
                            string ext = Path.GetExtension(fileName);

        

                            fileName = FileHelper.GetFileName(Purpose, ext);

                            FileUpload newFile = new FileUpload()
                            {
                                File = file,
                                UploadName = fileName,
                                IsImage = true,
                                EncryptFile = EncryptFile
                            };

                            string retour = FileHelper.UploadFile(newFile);
                            if (retour != "KO")
                            {
                                _success = true;
                                _PathFile = retour;
                                if (newFile.EncryptFile)
                                {
                                    bool IsUserPicture = false;
                                    if (Purpose.ToLower().Contains("user"))
                                    {
                                        IsUserPicture = true;
                                    }
                                    _PathFilePreview = FileHelper.GetDecryptedFilePath(retour, IsUserPicture);

                                }
                                else
                                {
                                    _PathFilePreview = _PathFile.Replace("~", "");
                                }
                            }
                        }
                        else
                        {
                            _Error = "[[[The document must be a picture or a pdf.]]]";
                        }
                    }
                    else
                    {
                        _Error = "[[[File must be smaller than ]]]" + CommonsConst.FileSize.ToString(SizeLimit.Value);
                    }

                }

            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Purpose = " + Purpose + " and EncryptFile = " + EncryptFile);
            }
            return Json(new { Result = _success, PathFile = _PathFile, Error = _Error, PathFilePreview = _PathFilePreview });
        }

        /// <summary>
        /// Download an envrypted file
        /// </summary>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult DownloadFile(string PathFile)
        {

            try
            {
                if (!String.IsNullOrWhiteSpace(PathFile))
                {
                    byte[] fileBytes = FileHelper.GetFileToDownLoad(PathFile);
                    string ext = Path.GetExtension(PathFile);
                    var fileName = Path.GetFileName(PathFile);

                    string contentType = MimeMapping.GetMimeMapping(fileName);
                    return File(fileBytes, contentType, fileName);
                }

            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "PathFile =" + PathFile);
            }
            return null; ;
        }

        /// <summary>
        /// Delete the specified file
        /// </summary>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(string PathFile)
        {
            bool _success = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(PathFile))
                {
                    _success = FileHelper.DeleteDocument(PathFile);
                }

            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "PathFile =" + PathFile);
            }
            return Json(new { Result = _success });
        }

    }
}
