using Quartz;
using System;
using System.Net;
using System.Net.Mail;
using Commons;

using Models.Class;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Class.FileUpload;
using Service.Admin.Interface;
using Service.Admin;
using DataEntities.Model;

namespace Service.TaskClasses
{
    public class DeleteUploadedFile : RecurringJobBase
    {
        FileUploadService _fileUploadServiceService { get; set; }

        public DeleteUploadedFile()
        {
            _fileUploadServiceService = new FileUploadService();
        }


        public override void Execute(IJobExecutionContext context)
        {
            try
            {
                base.Execute(context);
                FileUploadDeleteResult ObjectResult = _fileUploadServiceService.DeleteUploadFiles();
                TaskLog Log = new TaskLog();
                Log.Id = LogId;
                string Comment = "N/A";
                bool Result = false;
                if (ObjectResult != null)
                {
                    Result = ObjectResult.Result;
                    Comment = ObjectResult.FilesAnalyzedNumber + " files analyzed : </br> - " + ObjectResult.FilesDeletedNumber + " files deleted </br> - " + ObjectResult.FilesErrorsNumber + " errors";
                }

                _taskLogService.UpdateTaskLog(LogId,Result,Comment);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}