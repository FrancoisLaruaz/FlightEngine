using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FlightsEngine.Models;
using Transavia.Api.FlightOffers.Client;
using Transavia.Api.FlightOffers.Client.Model;
using System.Web.Script.Serialization;
using System.Threading;
using System.Net;
using System.Windows.Forms;

namespace FlightsEngine.FlighsBot
{
    public static class Kayak
    {


        public static bool SearchFlights(AirlineSearch filter)
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START Kayak ***");
                MakeRequest(filter);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Kayak ***");
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filter.ToSpecialString());
            }
            return result;
        }




        static void MakeRequest(AirlineSearch filters)
        {
            try
            {
                string url = "https://www.kayak.com/flights/AMS-BCN/2018-10-20/2018-10-28?sort=bestflight_a&fs=flexdepart=20181020;flexreturn=20181028";
                url= "https://www.ca.kayak.com/?uuid=89d5d6c0-9bec-11e8-8c5b-df3051146d6e&vid=&url=%2Fflights%2FAMS-BCN%2F2018-10-20%2F2018-10-28%3Fsort%3Dbestflight_a%26fs%3Dflexdepart%3D20181020%3Bflexreturn%3D20181028";
                WebRequest request = WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
            }
        }
    }
}
