using Commons;

using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.Class.FileUpload;

namespace Service.Admin.Interface
{
    public interface IFileUploadService
    {
        FileUploadDeleteResult DeleteUploadFiles();

    }
}
