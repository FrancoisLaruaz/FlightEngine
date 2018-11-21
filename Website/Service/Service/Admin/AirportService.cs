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
using Service.Admin.Interface;

namespace Service.Admin
{
    public  class AirportService : IAirportService
    {

        private  string WebsiteURL = ConfigurationManager.AppSettings["Website"];

        private readonly IGenericRepository<Airport> _airportRepo;


        public AirportService(IGenericRepository<DataEntities.Model.Airport> airportRepo)
        {
            _airportRepo = airportRepo;
        }

        public AirportService()
        {
            var context = new TemplateEntities();
            _airportRepo = new GenericRepository<DataEntities.Model.Airport>(context);
        }




    }
}
