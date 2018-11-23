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
using Models.Class.API;
using Models.Class.Weather;

namespace Service.Admin
{
    public class WeatherService : IWeatherService
    {

        private string WebsiteURL = ConfigurationManager.AppSettings["Website"];

        private readonly IGenericRepository<Airport> _airportRepo;
        private readonly IGenericRepository<HistoricWeather> _historicWeatherRepo;
        private readonly IGenericRepository<City> _cityRepo;

        public WeatherService(IGenericRepository<DataEntities.Model.Airport> airportRepo,
            IGenericRepository<HistoricWeather> historicWeatherRepo,
            IGenericRepository<City> cityRepo)
        {
            _airportRepo = airportRepo;
            _historicWeatherRepo = historicWeatherRepo;
            _cityRepo = cityRepo;
        }

        public WeatherService()
        {
            var context = new TemplateEntities();
            _airportRepo = new GenericRepository<DataEntities.Model.Airport>(context);
            _historicWeatherRepo = new GenericRepository<DataEntities.Model.HistoricWeather>(context);
            _cityRepo = new GenericRepository<DataEntities.Model.City>(context);
        }



        public bool AddHistoricWeather(HistoricWeatherItem item, DateTime Date, int AirportId)
        {
            bool result = true;
            try
            {
                var data = item?.daily?.data?.FirstOrDefault();
                if (data != null)
                {
                    HistoricWeather historicWeather = new HistoricWeather();
                    historicWeather.Date = Date;
                    historicWeather.Summary = data.summary;
                    historicWeather.Icon = data.icon;
                    historicWeather.PrecipType = data.precipType;
                    historicWeather.PrecipIntensity = Convert.ToDecimal(data.precipIntensity);
                    historicWeather.PrecipProbability = Convert.ToDecimal(data.precipProbability);
                    historicWeather.TemperatureHigh = WeatherHelper.GetCelciusTemperature(Convert.ToDecimal(data.temperatureHigh));
                    historicWeather.TemperatureLow = WeatherHelper.GetCelciusTemperature(Convert.ToDecimal(data.temperatureLow));
                    historicWeather.Humidity = Convert.ToDecimal(data.humidity);
                    historicWeather.WindSpeed = Convert.ToDecimal(data.windSpeed);
                    historicWeather.CloudCover = Convert.ToDecimal(data.cloudCover);
                    historicWeather.AirportId = AirportId;
                    _historicWeatherRepo.Add(historicWeather);
                    result = _historicWeatherRepo.Save();
                }


            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "AirportId = " + AirportId + " and Date = " + Date);
            }

            return result;
        }

        public bool SetAirportWeather()
        {
            bool result = true;
            try
            {

                List<APIKey> Keys = new List<APIKey>();
                Keys.Add(new APIKey("490e854a3514ea0f7f1810463dabc647"));
                Keys.Add(new APIKey("1df9017a2a7dba1a7d71f38cfecefb54"));
                Keys.Add(new APIKey("9cdd30b4010b31748a8525bc8b334e60"));
                Keys.Add(new APIKey("35f7cb7530eb07d0f3b47ca49182445d"));
                Keys.Add(new APIKey("6841a3f8d7d154552d2605d7be209351"));
                Keys.Add(new APIKey("5f12421d192dea47c28585df8eccafd6"));
                Keys.Add(new APIKey("8ae34f0b0046acdef3f1355c1317e50f"));
                Keys.Add(new APIKey("3dc43f217289d59c9d068b4d8a72f81b"));
                Keys.Add(new APIKey("159b8f6f025859c35b579033213d7695"));
                Keys.Add(new APIKey("5f373aa939f7c088a043051941357de6"));
                Keys.Add(new APIKey("b0fd8436af42202a5849cfb1e70af9b6"));
                Keys.Add(new APIKey("be3336318ad232ff56defc4588295f27"));
                Keys.Add(new APIKey("1e91dbdda7efef52c832979aea8dab66"));
                Keys.Add(new APIKey("f86051b3243ed66e705cce8f434f0e0b"));
                Keys.Add(new APIKey("a2257d9a0af19152e2c6cfccbc3fa498"));
                Keys.Add(new APIKey("763a4c51d0afce69a17891e9ffc9dbb4"));
                Keys.Add(new APIKey("d205806e3d5a1d900859cc3d82febb96"));
                Keys.Add(new APIKey("7b9995c4df3be3df39053f4df6f74965"));
                Keys.Add(new APIKey("566b55cfeee56e43136a17e3bb7896c5"));
                Keys.Add(new APIKey("fada50cfd88b7364f0de7272c0446f90"));
                Keys.Add(new APIKey("1dc8c93c862d080d435e23e8441a4a5f"));
                Keys.Add(new APIKey("ad8e91aa8511ac2e68a28b43670d94db"));
                Keys.Add(new APIKey("703708d9bb26be5db461cb55df9c322a"));
                Keys.Add(new APIKey("6c68181da024a1e6ffe067bb91ba81cd"));
                Keys.Add(new APIKey("bd3fded169268ba8af871490c0db3990"));
                Keys.Add(new APIKey("a99f8b9cab49975c659a27f9830557df"));
                Keys.Add(new APIKey("acfce707d4ae405210e032e63f260b2d"));
                Keys.Add(new APIKey("1ea3d77ef1f1086da23b08bfeaa126d1"));
                Keys.Add(new APIKey("de854affd68a2fca04670f309945d1e5"));
                Keys.Add(new APIKey("f6b966f0b40949d8d5f6f64f7ea1f7fe"));
                Keys.Add(new APIKey("3738d4cf9a5225703577fde59ef21d72"));
                Keys.Add(new APIKey("c3085013199ba2d249405f5506eaa0f5"));
                Keys.Add(new APIKey("c37cc8d06de6118f10372198e4f1eca9"));
                Keys.Add(new APIKey("1583cf425f7255ad532249ba590152e3"));
                Keys.Add(new APIKey("d354ca951d6d7f556ec8cd171a2d357c"));

                List<Airport> Airports = _airportRepo.FindAllBy(a => a.Active).ToList();
                //Airports = Airports.Where(a => a.Id == 7921 || a.Id == 2565).ToList();

                DateTime Start = new DateTime(2015, 01, 01);
                DateTime End = new DateTime(2017, 12, 31);
                DateTime LastDataFetched = _historicWeatherRepo.List()?.OrderByDescending(h => h.Date)?.FirstOrDefault()?.Date ?? Start;
                DateTime Date = LastDataFetched;
                bool MaxAPIRequestsReached = false;
                while (Date <= End && !MaxAPIRequestsReached)
                {
                    string UnixDate = DateHelper.GetUnixTimeStamp(Date);
                    foreach (var airport in Airports)
                    {
                        if (Date > LastDataFetched || (!_historicWeatherRepo.FindAllBy(w => w.AirportId == airport.Id && w.Date == Date).Any()))
                            try
                            {
                                if (airport.Longitude != 0 && airport.Latitude != 0)
                                {
                                    APIKey KeyToUse = Keys.Where(k => k.RequestsNumber < 1000).OrderBy(k => k.RequestsNumber).FirstOrDefault();
                                    if (KeyToUse != null)
                                    {
                                        HistoricWeatherItem weather = WeatherHelper.GetWeather(airport.Latitude.Value, airport.Longitude.Value, UnixDate, KeyToUse.Key);
                                        Keys.Where(k => k.Key == KeyToUse.Key).FirstOrDefault().RequestsNumber = KeyToUse.RequestsNumber + 1;
                                        AddHistoricWeather(weather, Date, airport.Id);
                                    }
                                    else
                                    {
                                        // We stop
                                        MaxAPIRequestsReached = true;
                                    }
                                }
                            }
                            catch (Exception e2)
                            {
                                result = false;
                                Commons.Logger.GenerateError(e2, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Airport weather Loop, Id = " + airport.Id);
                            }
                    }
                    Date = Date.AddDays(1);
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
