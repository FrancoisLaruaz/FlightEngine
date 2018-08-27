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

namespace Commons
{

    public static class GeoLocalizationHelper
    {

        public static double GetDistance(LocalizationItem from, LocalizationItem to)
        {
            try
            {
                var sCoord = new GeoCoordinate(from.numLat, from.numLon);
                var eCoord = new GeoCoordinate(to.numLat, to.numLon);

                return sCoord.GetDistanceTo(eCoord);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return -1;
        }

        public static LocalizationItem GetLocalizationItemFromIpAddress(string ipAddress)
        {
            string _ipAddress = ipAddress;
            LocalizationItem ipdata = null;
            try
            {
                
                if (Utils.IsLocalhost())
                {
                    ipAddress = "199.60.221.180";
                }

                if (!String.IsNullOrWhiteSpace(ipAddress))
                {
                    WebClient client = new WebClient();
                    string response = client.DownloadString("http://ip-api.com/json/" + ipAddress);
                    if (response != null)
                    {
                        ipdata = JsonConvert.DeserializeObject<LocalizationItem>(response);
                        if (ipdata!=null && ipdata.status != "fail")
                        {
                            ipdata.numLon = Convert.ToDouble(ipdata.lon);
                            ipdata.numLat = Convert.ToDouble(ipdata.lat);
                            ipdata.userHostAddress = _ipAddress;
                            return ipdata;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ipAddress = " + ipAddress);
            }
            return new LocalizationItem(49.2788, -123.1139, "Vancouver", _ipAddress); ;

        }
    }
}
