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
        public const string DefaultCurrency ="USD";

    }

    public static class HTTPStatus
    {
        public const string OK = "200";
        public const string BqdRequest = "400";
        public const string InternalServerError = "500";

    }

    public static class PythonError
    {
        public const string WebdriverTimeout = "waitForWebdriver";

    }

    public static class AIRFranceKLMTravelHost
    {
        public const string KL = "KL";
        public const string AF = "AF";

    }
}
