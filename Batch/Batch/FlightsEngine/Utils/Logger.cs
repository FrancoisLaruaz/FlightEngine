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
using System.Net;
using log4net;

namespace FlightsEngine.Utils
{
    public static class Logger
    {



        public static List<string> NotLoggedErrorsUrl = new List<string> { };
        public static List<string> NotLoggedErrorsMessage = new List<string> {  };


        public static bool LoggeError(string Message)
        {
            bool result = true;
            try
            {

                if (result && NotLoggedErrorsMessage != null && Message != null)
                {
                    foreach (string pattern in NotLoggedErrorsMessage)
                    {
                        if (Message.Contains(pattern))
                            result = false;
                    }
                }
            }
            catch
            {
                result = true;
            }
            return result;
        }

        public static void GenerateWebError(WebException e, System.Type type = null, string Details = null)
        {
            try
            {
                var response = ((HttpWebResponse)e.Response);

                var reader = new StreamReader(e.Response.GetResponseStream());
                string error = "WEB EXCEPTION :  error type : " + e.Status ;
                if (reader != null)
                    error = error + " : " + reader.ReadToEnd();
                error= error + " and " + Details;
                GenerateError(e, type, error);
            }
            catch (WebException ex2)
            {
                if (ex2 == null)
                {
                    Logger.GenerateInfo("Error while creating a web Log.");
                }
                else
                {
                    Logger.GenerateInfo("Error while creating a web Log : " + ex2?.ToString());
                }
            }
        }

        public static void GenerateError(Exception Ex, System.Type type = null, string Details = null)
        {
            bool LoggeError = true;

            try
            {
                if (Ex == null)
                {
                    Ex = new Exception("Custom Error");
                }
                log4net.ILog logger = null;
                if (type == null)
                {
                    logger = log4net.LogManager.GetLogger("Unkwnon");
                }
                else
                {
                    logger = log4net.LogManager.GetLogger(type);
                }

                string Message = "";

                LoggeError = Logger.LoggeError(Ex?.Message);


                if (LoggeError)
                {

                    if (!String.IsNullOrEmpty(Details))
                        Message = "- Details => " + Details + " </br></br>" + Message;

                    Message = "- Message => " + Ex?.Message + " </br></br>" + Message;
                    logger.Error(Message, Ex);
                }
                Console.WriteLine(Ex.ToString() + " " + (Details ?? ""));
            }
            catch (Exception ex2)
            {
                if (ex2 == null)
                {
                    Logger.GenerateInfo("Error while creating a Log.");
                }
                else
                {
                    Logger.GenerateInfo("Error while creating a Log : " + ex2?.ToString());
                }
            }

        }

        /// <summary>
        /// Generate an Info
        /// </summary>
        /// <param name="Message"></param>
        public static void GenerateInfo(string Message)
        {
            try
            {
                log4net.ILog logger = log4net.LogManager.GetLogger("*** EVENT ***");
                logger.Info(Message);
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, typeof(Logger));
            }
        }

        public static ILog Monitoring
        {
            get
            {

                return LogManager.GetLogger("MonitoringLogger");
            }
        }
        public static ILog Generation
        {
            get
            {
                return LogManager.GetLogger("GenerationLogger");
            }
        }
    }
}
