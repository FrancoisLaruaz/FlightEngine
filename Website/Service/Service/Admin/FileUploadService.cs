using Commons;
using Models.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Class.FileUpload;
using Models.ViewModels;
using DataEntities.Repositories;
using DataEntities.Model;
using Service.Admin.Interface;

namespace Service.Admin
{
    public  class FileUploadService : IFileUploadService
    {

        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Product> _productRepo;

        public FileUploadService(IGenericRepository<User> userRepo, IGenericRepository<Product> productRepo)
        {
            _userRepo = userRepo;
            _productRepo = productRepo;
        }

        public FileUploadService()
        {
            var context = new TemplateEntities();
            _userRepo = new GenericRepository<User>(context);
            _productRepo= new GenericRepository<Product>(context);
        }

        /// <summary>
        /// Delete old files not used
        /// </summary>
        /// <returns></returns>
        public  FileUploadDeleteResult DeleteUploadFiles()
        {
            FileUploadDeleteResult result = new FileUploadDeleteResult();
            try
            {
                int intDate;
                DirectoryInfo InfoUploadFolderEncrypted = new DirectoryInfo(FileHelper.GetStorageRoot(CommonsConst.Const.BasePathUploadEncrypted));//Assuming Test is your Folder
                DirectoryInfo InfoUploadFolderDecrypted = new DirectoryInfo(FileHelper.GetStorageRoot(CommonsConst.Const.BasePathUploadDecrypted));//Assuming Test is your Folder
                List<FileInfo> Files = InfoUploadFolderEncrypted.GetFiles().ToList(); //Getting Text 
                List<FileInfo> FilesDecrypted = InfoUploadFolderDecrypted.GetFiles().Where(f => !f.Name.Contains("news")).ToList(); //Getting Text files
                Files.AddRange(FilesDecrypted);
                string FileName = "";
                DateTime DateToCompare = DateTime.UtcNow.AddDays(-2);

                var UsersList = _userRepo.List();
                var ProductsList = _productRepo.List();

                List<string> ListUsedFilesPictureSrc = UsersList.Select(u => u.PictureSrc)?.ToList();
                List<string> ListUsedFilesPictureThumbnailSrc = UsersList.Select(u => u.PictureThumbnailSrc)?.ToList();
                List<string> ListUsedFilesProductsImageSrc = ProductsList.Select(u => u.ImageSrc)?.ToList();

                List<string> ListUsedFiles = ListUsedFilesPictureSrc.Concat(ListUsedFilesPictureThumbnailSrc).Concat(ListUsedFilesProductsImageSrc).ToList();

                if (ListUsedFiles != null && ListUsedFiles.Count > 0)
                {
                    foreach (FileInfo file in Files)
                    {
                        result.FilesAnalyzedNumber++;
                        FileName = file.Name;
              
                        try
                        {
                            if (FileName.Length < 8 && int.TryParse(FileName.Substring(0, 8).ToString(), out intDate))
                            {
                                result.FilesErrorsNumber++;
                            }
                            else
                            {
                                DateTime DateFile = new DateTime(Convert.ToInt32(FileName.Substring(0, 4)), Convert.ToInt32(FileName.Substring(4, 2)), Convert.ToInt32(FileName.Substring(6, 2)));
                                string FullPathFile = CommonsConst.Const.BasePathUploadEncrypted + "/" + FileName;

                                if(file.FullName.Contains("Decrypted"))
                                {
                                    FullPathFile = CommonsConst.Const.BasePathUploadDecrypted + "/" + FileName;
                                }
            

                                if (DateFile < DateToCompare && !ListUsedFiles.Contains(FullPathFile))
                                {
                                    bool resultDelete = FileHelper.DeleteDocument(FullPathFile);
                                    if (resultDelete)
                                    {
                                        result.FilesDeletedNumber++;
                                    }
                                    else
                                    {
                                        result.FilesErrorsNumber++;
                                    }
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            result.FilesErrorsNumber++;
                            Commons.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FileName = " + FileName);
                        }
                    }
                }

                if (result.FilesErrorsNumber > 0)
                    result.Result = false;
                else
                    result.Result = true;
            }
            catch (Exception e)
            {
                result.Result = false;
                result.Error = "Error : " + e.ToString();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

    }

}
