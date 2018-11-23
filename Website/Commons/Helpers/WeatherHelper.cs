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
using Models.Class.Localization;
using System.Device.Location;
using Newtonsoft.Json;
using Models.Class.Weather;

namespace Commons
{

    public static class WeatherHelper
    {

        public static decimal GetCelciusTemperature(decimal FahrenheitTemperature)
        {
            decimal result = -1;
            try
            {
                result = (FahrenheitTemperature - 32) * 5 / 9;
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FahrenheitTemperature = " + FahrenheitTemperature);
            }
            return result;
        }


        public static HistoricWeatherItem GetWeather(float Latitude, float Longitude,string UnixDate,string Key)
        {
            HistoricWeatherItem result = new HistoricWeatherItem();
            try
            {
                //https://api.darksky.net/forecast/490e854a3514ea0f7f1810463dabc647/49.19389,-123.1844,1542684965?exclude=currently,flags,hourly
                // https://darksky.net/dev/docs#time-machine-request

                bool success = false;
                int nbAttempts = 1;
                int nbMaxAttempts = 4;

                while (!success && nbAttempts <= nbMaxAttempts)
                {
                    nbAttempts++;
                    WebClient client = new WebClient();
                    string url = "https://api.darksky.net/forecast/" + Key + "/" + Latitude.ToString().Replace(",",".") + "," + Longitude.ToString().Replace(",", ".") + "," + UnixDate + "?exclude=currently,flags,hourly";
                    string response = client.DownloadString(url);
                    if (response != null)
                    {
                        result = JsonConvert.DeserializeObject<HistoricWeatherItem>(response);
                        success = true;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Latitude = " + Latitude+ " and Longitude= "+ Longitude+ " and UnixDate = " + UnixDate);
            }
            return result;

        }
    }
}
