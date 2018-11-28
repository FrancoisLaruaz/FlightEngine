using FlightsEngine.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch1
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " *  START BATCH *");
                log4net.Config.XmlConfigurator.Configure();

     
                int? ProviderId = null;
                int? SearchTripWishesId = null;
                if (args.Length >= 1)
                {
                    SearchTripWishesId = Convert.ToInt32(args[0]);
                    if(SearchTripWishesId<0)
                    {
                        SearchTripWishesId = null;
                    }
                }
                if (args.Length >= 2)
                {
                    ProviderId = Convert.ToInt32(args[1]);
                }

               bool result = FlightsEngine.Program.SearchFlights(SearchTripWishesId, ConfigurationManager.AppSettings["ScrappingFolder"], ConfigurationManager.AppSettings["FirefoxExeFolder"], ProviderId);
                //bool result = FlightsEngine.Program.ResetConfig();

                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " *  END BATCH *");
                if (result)
                {
                    Console.WriteLine("OK");
                }
                else
                {
                    Console.WriteLine("KO");
                }

                if (ConfigurationManager.AppSettings["ExitWhenFinished"] == "NO")
                {
                    Console.WriteLine("Press Enter to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, string.Join(" , ", args));
                Console.WriteLine("Error : " + e.ToString());
                Console.WriteLine("KO");
            }
        }
    }
}
