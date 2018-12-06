using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;

namespace FlightsEngine.Models.Constants
{
    public static class Constants
    {
        public const string DefaultCurrency ="CAD";
        public const int MaxRequestAttempts = 3;
        public const int APIMaxDaysNumberForSearch = 70;
        public const string Separator = " - ";
    }

    public static class HTTPStatus
    {
        public const string OK = "200";
        public const string BqdRequest = "400";
        public const string InternalServerError = "500";

    }

    public static class DateFormat
    {
        public const string Trip = "dd/MM/yyyy HH:mm";
    }

    public static class Providers
    {
        public const int Edreams = 1;
        public const int Kayak = 2;
        public const int Kiwi = 3;
        public const int AirFrance = 4;
        public const int KLM = 5;
        public const int Transavia =6;
        public const int TurkishAirlines = 7;
        public const int RyanAir = 8;

        public static string ToString(int value)
        {
            string result = "N/A";

            if(value==Edreams)
            {
                result = "Edreams";
            }
            else if (value == Kayak)
            {
                result = "Kayak";
            }
            else if (value == Kiwi)
            {
                result = "Kiwi";
            }
            else if (value == AirFrance)
            {
                result = "Air France";
            }
            else if (value == KLM)
            {
                result = "KLM";
            }
            else if (value == Transavia)
            {
                result = "Transavia";
            }
            else if (value == TurkishAirlines)
            {
                result = "Turkish Airlines";
            }
            else if (value == RyanAir)
            {
                result = "Ryan Air";
            }
            return result;
        }

    }

    public static class PythonError
    {
        public const string WebdriverTimeout = "waitForWebdriver";

    }

    public static class AIRFranceKLMTravelHost
    {
        public const string KL = "KL";
        public const string AF = "AF";
        public static string ToString(string value)
        {
            string result = "N/A";

            if (value == KL)
            {
                result = "KLM";
            }
            else if (value == AF)
            {
                result = "Air France";
            }


            return result;
        }
    }
}
