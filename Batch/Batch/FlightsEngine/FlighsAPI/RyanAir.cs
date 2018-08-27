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

namespace FlightsEngine.FlighsAPI
{
    public static class RyanAir
    {
        public static string Key = "TaiRJ5T7SFngdrJkiGob9InuEFE4xeJK";
        public static string SecretKey = "7mOsWXal1132p1av";


        public static bool SearchFlights(AirlineSearch filter)
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START Ryan Air  ***");
                MakeRequest(filter);

                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Ryan Air  ***");
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filter.ToSpecialString());
            }
            return result;
        }


        static bool MakeRequest(AirlineSearch filters)
        {
            bool result = false;
            try
            {
                string url = "";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Api-Key", Key);


                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    /*
                    // Request parameters
                    if (!String.IsNullOrWhiteSpace(filters.AirportDepartureCode))
                        queryString["Origin"] = filters.AirportDepartureCode;
                    if (!String.IsNullOrWhiteSpace(filters.AirportArrivalCode))
                        queryString["Destination"] = filters.AirportArrivalCode;
                    if (filters.DepartureDate != null)
                        queryString["OriginDepartureDate"] = filters.DepartureDate.Value.ToString("yyyyMMdd");
                    if (filters.ArrivalDate != null)
                        queryString["OriginArrivalDate"] = filters.ArrivalDate.Value.ToString("yyyyMMdd");
                    queryString["DirectFlight"] = filters.DirectFlightsOnly.ToString().ToLower();

                    if (filters.AdultsNumber != null)
                        queryString["Adults"] = filters.AdultsNumber.ToString();
                    if (filters.ChildrenNumber != null)
                        queryString["Children"] = filters.ChildrenNumber.ToString();


                    HubSpotJsonObject jsonObject = new HubSpotJsonObject();
                    jsonObject.properties.Add(new Property("email", contact.Email));
                    jsonObject.properties.Add(new Property("firstname", contact.FirstName));
                    jsonObject.properties.Add(new Property("lastname", contact.LastName));
                    jsonObject.properties.Add(new Property("website", contact.WebSite));
                    jsonObject.properties.Add(new Property("company", contact.Company));
                    jsonObject.properties.Add(new Property("language", contact.Language));
                    jsonObject.properties.Add(new Property("address", contact.Address));
                    jsonObject.properties.Add(new Property("city", contact.City));
                    jsonObject.properties.Add(new Property("state", contact.State));
                    jsonObject.properties.Add(new Property("zip", contact.Zip));
                    jsonObject.properties.Add(new Property("gender", contact.Gender));
                    jsonObject.properties.Add(new Property("birth_date", contact.BirthDate));
                    jsonObject.properties.Add(new Property("last_connection_date", contact.LastConnectionDate));
                    jsonObject.properties.Add(new Property("phone", contact.PhoneNumber));
                    jsonObject.properties.Add(new Property("country", contact.Country));
                    if (contact.CompletedInvestmentsNumber != null)
                        jsonObject.properties.Add(new Property("completed_investments_number", contact.CompletedInvestmentsNumber?.ToString()));
                    if (contact.CompletedInvestmentsSum != null)
                        jsonObject.properties.Add(new Property("completed_investments_sum", contact.CompletedInvestmentsSum?.ToString()));
                    jsonObject.properties.Add(new Property("hasffaccount", "true"));
                    jsonObject.properties.Add(new Property("investor_status", contact.InvestorStatus));
                    jsonObject.properties.Add(new Property("import_data_from_ff_website", contact.Import_data_from_ff_website));
                    jsonObject.properties.Add(new Property("ff_investor_profile_url", contact.ff_investor_profile_url));
                    jsonObject.properties.Add(new Property("ff_admin_investor_profile_url", contact.ff_admin_investor_profile_url));
                    jsonObject.properties.Add(new Property("receive_new_campaign_launches_emails", contact.Receive_new_campaign_launches_emails));
                    jsonObject.properties.Add(new Property("receive_monthly_news_letter", contact.Receive_monthly_news_letter));
                    jsonObject.properties.Add(new Property("receive_promotional_emails", contact.Receive_promotional_emails));
                    jsonObject.properties.Add(new Property("receive_followed_companies_email", contact.receive_followed_companies_email));
                    jsonObject.properties.Add(new Property("receiveb_blog_emails", contact.Receiveb_blog_emails));
                    jsonObject.properties.Add(new Property("receive_crypto_updates", contact.Receive_crypto_updates));
                    jsonObject.properties.Add(new Property("receive_investment_reminders", contact.Receive_investment_reminders));
                    jsonObject.properties.Add(new Property("has_downloaded_the_funding_guide", contact.Has_downloaded_the_funding_guide));
                    string json = new JavaScriptSerializer().Serialize(jsonObject);
                    */
                    string json = "ere {\"TripType\": \"O\", \"NoOfAdults\": 1, \"NoOfChilds\": 0, \"NoOfInfants\": 0, \"ClassType\": \"Economy\", \"OriginDestination\": [ { \"Origin\": \"CDG\", \"Destination\": \"BCN\", \"TravelDate\": \"08/08/2018\" } ], \"Currency\": \"USD\" }";
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var requestResult = streamReader.ReadToEnd();

                    if (!String.IsNullOrWhiteSpace(requestResult))
                    {
                        JavaScriptSerializer srRequestResult = new JavaScriptSerializer();
                        dynamic jsondataRequestResult = srRequestResult.DeserializeObject(requestResult);
                        if (jsondataRequestResult != null && FlightsEngine.Utils.Utils.IsPropertyExist(jsondataRequestResult, ""))
                        {
                            result = true;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
            }
            return result;
        }

 
    }
}
