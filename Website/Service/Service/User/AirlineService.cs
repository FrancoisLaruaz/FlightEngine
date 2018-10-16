using Commons;
using Models.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Models.ViewModels;
using System.IO;
using CommonsConst;
using Service.UserArea.Interface;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.Class.Email;
using Models.ViewModels.Admin.Email;

namespace Service.UserArea
{
    public  class AirlineService : IAirlineService
    {

        private  string WebsiteURL = ConfigurationManager.AppSettings["Website"];

        private readonly IGenericRepository<Airline> _airlineRepo;


        public AirlineService(IGenericRepository<DataEntities.Model.Airline> airlineRepo)
        {
            _airlineRepo = airlineRepo;
        }

        public AirlineService()
        {
            var context = new TemplateEntities();
            _airlineRepo = new GenericRepository<DataEntities.Model.Airline>(context);
        }

        public bool DownloadAirlinesImages()
        {
            bool result = true;
            try
            {
                List<Airline> AIrlines = _airlineRepo.FindAllBy(a => a.ImageSrc.Contains("http")).ToList();
                foreach(var airline in AIrlines)
                {
                    airline.ImageSrc = FileHelper.SaveFileFromWeb(airline.ImageSrc.Split('?')[0],"Airline");
                    _airlineRepo.Edit(airline);
                    result = result & _airlineRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }



    }
}
