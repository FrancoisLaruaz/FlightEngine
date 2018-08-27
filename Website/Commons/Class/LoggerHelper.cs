using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace Commons
{
    public static class Logger
    {


        public static List<string> NotLoggedErrorsUrl = new List<string> { };
        public static List<string> NotLoggedErrorsMessage = new List<string> { "Le contrôleur pour le chemin" };


        /// <summary>
        /// Save into the database a javscript error
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="URLError"></param>
        /// <param name="lineNumber"></param>
        /// <param name="col"></param>
        /// <param name="error"></param>
        /// <param name="browser"></param>
        /// <param name="custom"></param>
        public static void GenerateJavascriptError(string Message = "", string URLError = "", string lineNumber = "", string col = "", string error = "", string browser = "",bool custom=false)
        {
            try
            {

                log4net.ILog logger = log4net.LogManager.GetLogger("*** JAVASCRIPT ***");
                string UrlReferrer = HttpContext.Current?.Request?.UrlReferrer?.AbsoluteUri;
                bool LoggeError = Logger.IsErrorLogged(URLError, Message) && Logger.IsErrorLogged(UrlReferrer, Message);

                if (LoggeError && (!String.IsNullOrEmpty(Message) || !String.IsNullOrEmpty(UrlReferrer) || !String.IsNullOrEmpty(URLError) || !String.IsNullOrEmpty(error)))
                {
                    string MessageToLog = "";
                    if (custom)
                    {
                        MessageToLog = MessageToLog + "*** Custom Error ***";
                        MessageToLog = MessageToLog + " </br></br>- Message => " + Message;
                    }
                    else
                    {
                        MessageToLog = MessageToLog + "- Message => " + Message;
                    }
                    if (!String.IsNullOrEmpty(URLError))
                        MessageToLog = MessageToLog + " </br></br>- URL Error => " + URLError;
                    if (!String.IsNullOrEmpty(UrlReferrer) && (String.IsNullOrEmpty(URLError) || URLError.Trim().ToLower() != UrlReferrer.Trim().ToLower()))
                        MessageToLog = MessageToLog + " </br></br>- URL Referrer => " + UrlReferrer;
                    if (!String.IsNullOrEmpty(lineNumber))
                        MessageToLog = MessageToLog + " </br></br>- Line Number => " + lineNumber;
                    if (!String.IsNullOrEmpty(col))
                        MessageToLog = MessageToLog + " </br></br>- Col => " + col;
                    if (!String.IsNullOrEmpty(browser))
                        MessageToLog = MessageToLog + " </br></br>- Browser => " + browser;
                    if (!String.IsNullOrEmpty(error))
                        MessageToLog = MessageToLog + " </br></br>- Error => " + error;
                    logger.Error(MessageToLog);
                }

            }
            catch (Exception ex)
            {
                if (ex == null)
                {
                    Logger.GenerateInfo("Error while creating a javascript Log.");
                }
                else
                {
                    Logger.GenerateInfo("Error while creating a javascript Log : " + ex?.ToString());
                }
            }
        }

        public static bool IsErrorLogged(string Url, string Message)
        {
            bool result = true;
            try
            {
                if (NotLoggedErrorsUrl != null && !String.IsNullOrWhiteSpace(Url))
                {
                    foreach (string pattern in NotLoggedErrorsUrl)
                    {
                        if (Url.Contains(pattern))
                            result = false;
                    }
                }
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

        /// <summary>
        /// Creation of a row in the database
        /// </summary>
        /// <param name="Ex"></param>
        /// <param name="type"></param>
        /// <param name="Details"></param>
        /// <param name="Url"></param>
        public static void GenerateError(Exception Ex, System.Type type = null, string Details = null, string Url = null)
        {
            bool LoggeError = true;
            try
            {


                log4net.ILog logger = null;
                if (type == null)
                {
                    logger = log4net.LogManager.GetLogger("Unkwnon");
                }
                else
                {
                    logger = log4net.LogManager.GetLogger(type);
                }
                Url = "Unknown";
                try                   
                {
                    Url = HttpContext.Current?.Request?.Url?.AbsoluteUri;
                }
                catch
                {
                    Url = "Unknown";
                }
                string ExceptionMessage = "";
                if (Ex != null)
                {
                    ExceptionMessage = Ex.Message;
                }
                else
                {
                    Ex = new Exception("Unknown exception");
                }

                string UrlReferrer = HttpContext.Current?.Request?.UrlReferrer?.AbsoluteUri;
                string Form = "";
                try
                {
                    Form = HttpContext.Current?.Request?.Form?.ToString();
                }
                catch
                {

                }
                string Message = "";

                LoggeError = Logger.IsErrorLogged(Url, ExceptionMessage) && Logger.IsErrorLogged(UrlReferrer, ExceptionMessage);

                if (LoggeError)
                {
                    if (!String.IsNullOrEmpty(Form) && !Utils.IsProductionWebsite())
                        Message = "- Form => " + Form;
                    if (!String.IsNullOrEmpty(Details))
                        Message = "- Details => " + Details + " </br></br>" + Message;
                    Message = "- HttpMethod => " + (HttpContext.Current?.Request?.HttpMethod ?? "N/A") + " </br></br>" + Message;
                    if (!String.IsNullOrEmpty(Url) && !String.IsNullOrEmpty(UrlReferrer) && UrlReferrer.Trim().ToLower() != Url.Trim().ToLower())
                        Message = "- Url Referrer => " + UrlReferrer + " </br></br>" + Message;
                    if (!String.IsNullOrEmpty(Url))
                        Message = "- Url => " + Url + " </br></br>" + Message;
                    Message = "- Message => " + ExceptionMessage + " </br></br>" + Message;
                    logger.Error(Message, Ex);
                }

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

            if (LoggeError && Utils.IsLocalhost() && (type != typeof(HttpApplication) || type == null))
            {
                throw Ex;
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
                if (String.IsNullOrWhiteSpace(Message))
                {
                    Message = "N/A";
                }
                string url = HttpContext.Current?.Request?.Url?.AbsoluteUri;
                if (!String.IsNullOrWhiteSpace(url))
                {
                    Message = "- Url => " + url + " </br></br>" + Message;
                }
                logger.Info(Message);
            }
            catch(Exception e)
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