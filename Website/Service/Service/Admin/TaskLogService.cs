using Commons;

using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Models.ViewModels;
using DataEntities.Model;
using Service.Admin.Interface;
using DataEntities.Repositories;
using Models.ViewModels.Admin.Tasks;

namespace Service.Admin
{
    public  class TaskLogService : ITaskLogService
    {
        private readonly IGenericRepository<DataEntities.Model.TaskLog> _taskLogRepo;

        public TaskLogService(IGenericRepository<DataEntities.Model.TaskLog> taskLogRepo)
        {
            _taskLogRepo = taskLogRepo;
        }

        public TaskLogService()
        {
            var context = new TemplateEntities();
            _taskLogRepo = new GenericRepository<TaskLog>(context);
        }

        public  DisplayTasksViewModel GetDisplayTasksViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplayTasksViewModel model = new DisplayTasksViewModel();
            try
            {

                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower().Trim();

 
                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    var FullTaskLogList = _taskLogRepo.List().ToList();
                    model.Count = FullTaskLogList.Count;
                    model.TaskList = FullTaskLogList.OrderByDescending(e => e.Id).Skip(StartAt).Take(PageSize).ToList();
                }
                else
                {
                    model.TaskList = _taskLogRepo.List().OrderByDescending(e => e.Id).ToList();
                }

                if (!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<TaskLog> resultIEnumerable = model.TaskList as IEnumerable<TaskLog>;
                    resultIEnumerable = resultIEnumerable.Where(a => (a.CallbackId != null && a.CallbackId.ToLower().Contains(Pattern)) || (a.Comment != null && a.Comment.ToLower().Contains(Pattern)) || (a.Result != null && a.Result.ToString().Contains(Pattern)) || a.Id.ToString().Contains(Pattern) || a.GroupName.Contains(Pattern) || 
                    ((FormaterHelper.GetFormatStringForDateDisplay(a.StartDate).ToLower().Contains(Pattern)) || FormaterHelper.GetFormatStringForDateDisplay(a.EndDate).ToLower().Contains(Pattern)));
                    model.Count = resultIEnumerable.ToList().Count;
                    model.TaskList = resultIEnumerable.OrderByDescending(a => a.Id).Skip(StartAt).Take(PageSize).ToList();
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }




        /// <summary>
        /// Insert a task
        /// </summary>
        /// <param name="Task"></param>
        /// <returns></returns>
        public int InsertTaskLog(TaskLog Task)
        {
            int Result = -1;
            try
            {
                _taskLogRepo.Add(Task);
                if (_taskLogRepo.Save())
                {
                    return Task.Id;
                }
            }
            catch (Exception e)
            {
                Result = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Result;
        }


        /// <summary>
        ///  Update a specified task
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Result"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public bool UpdateTaskLog(int Id,bool Result,string Comment)
        {
            bool Success = false;
            try
            {
                TaskLog task = _taskLogRepo.Get(Id);
                if (task != null)
                {
                    task.Result = Result;
                    task.Comment = Comment;
                    task.EndDate = DateTime.UtcNow;
                    _taskLogRepo.Edit(task);
                    return _taskLogRepo.Save();
                }
        
            }
            catch (Exception e)
            {
                Success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id);
            }
            return Success;
        }

    }
}
