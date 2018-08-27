using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Net;
using Models.Class.ExternalAuthentification;
using System.IO;
using Commons;
using System.Web.Script.Serialization;
using CommonsConst;
using Facebook;
using Newtonsoft.Json;

namespace Commons
{
    public static class SocialMediaHelper
    {

        public static ExternalSignUpInformation GetGoogleInformation(string Token)
        {
            ExternalSignUpInformation Result = new ExternalSignUpInformation();
            try
            {
                Result.LoginProvider = LoginProviders.Google;
                Result.EmailPermission = true;

                Uri apiRequestUri = new Uri("https://www.googleapis.com/oauth2/v2/userinfo?access_token=" + Token);
                //request profile image
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(apiRequestUri);
                    if(!String.IsNullOrWhiteSpace(json))
                    {
                        dynamic result = JsonConvert.DeserializeObject(json);
                        Result.ImageSrc = result.picture;
                        Result.Email = result.email;
                        Result.FirstName = result.given_name;
                        Result.LastName = result.family_name;
                        Result.ProviderKey = result.id;
                    }

                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Token =" + Token);
            }
            return Result;
        }

        public static ExternalSignUpInformation GetFacebookInformation(string Token)
        {
            ExternalSignUpInformation Result = new ExternalSignUpInformation();
            try
            {
                Result.LoginProvider = LoginProviders.Facebook;
                Result.EmailPermission = false;
                Uri targetUserUriPermission = new Uri("https://graph.facebook.com/me/permissions?access_token=" + Token);
                HttpWebRequest userPermission = (HttpWebRequest)HttpWebRequest.Create(targetUserUriPermission);

                // Read the returned JSON object response
                StreamReader userInfoPermission = new StreamReader(userPermission.GetResponse().GetResponseStream());
                string jsonResponsePermission = string.Empty;
                jsonResponsePermission = userInfoPermission.ReadToEnd();
                dynamic jsondataPermission = null;
                if (!string.IsNullOrWhiteSpace(jsonResponsePermission))
                {

                    JavaScriptSerializer srPermission = new JavaScriptSerializer();
                    jsondataPermission = srPermission.DeserializeObject(jsonResponsePermission);
                    if (jsondataPermission != null && Utils.DoesPropertyExist(jsondataPermission, "data"))
                    {
                        var DataPermission = jsondataPermission["data"];

                        foreach (var Perm in DataPermission)
                        {
                            if (Perm["permission"].ToString() == "user_friends")
                            {
                                if (Perm["status"].ToString() == "granted")
                                {
                                    Result.FriendsPermission = true;
                                }
                            }
                            else if (Perm["permission"].ToString() == "email")
                            {
                                if (Perm["status"].ToString() == "granted")
                                {
                                    Result.EmailPermission = true;
                                }
                            }
                        }
                    }
                }

                if (Result.EmailPermission)
                {
                    string jsonResponse = string.Empty;
                    if (Result.FriendsPermission)
                    {
                        try
                        {
                            var fb = new FacebookClient(Token);
                            dynamic fbFriends = fb.Get("/me/friends");
                            JavaScriptSerializer sr = new JavaScriptSerializer();
                            jsonResponse = fbFriends.data.ToString();
                            dynamic jsondata = sr.DeserializeObject(jsonResponse);
                            if (jsondata != null)
                            {
                                foreach (var friendItem in jsondata)
                                {
                                    string FacebookFriendId = friendItem["id"];
                                    Result.FriendsList.Add(FacebookFriendId);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Commons.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Get FriendsList, Token =" + Token);
                        }
                    }

                    Uri targetUserUri = new Uri("https://graph.facebook.com/me?fields=first_name,email,last_name,gender,locale,id,link&access_token=" + Token);
                    HttpWebRequest user = (HttpWebRequest)HttpWebRequest.Create(targetUserUri);

                    // Read the returned JSON object response
                    StreamReader userInfo = new StreamReader(user.GetResponse().GetResponseStream());
                    jsonResponse = string.Empty;
                    jsonResponse = userInfo.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(jsonResponse))
                    {

                        JavaScriptSerializer sr = new JavaScriptSerializer();
                        dynamic jsondata = sr.DeserializeObject(jsonResponse);
                        if (jsondata != null)
                        {
                            Result.FirstName = Utils.DoesPropertyExist(jsondata, "first_name") ? jsondata["first_name"].ToString() : null;
                            Result.LastName = Utils.DoesPropertyExist(jsondata, "last_name") ? jsondata["last_name"].ToString() : null;
                            Result.Email = Utils.DoesPropertyExist(jsondata, "email") ? jsondata["email"].ToString() : null;
                            Result.FacebookId = Utils.DoesPropertyExist(jsondata, "id") ? jsondata["id"].ToString() : null;
                            Result.ImageSrc = Utils.DoesPropertyExist(jsondata, "id") ? "http://graph.facebook.com/" + Result.FacebookId + "/picture?type=large&redirect=true&width=500&height=500" : null;
                            Result.FacebookLink = Utils.DoesPropertyExist(jsondata, "link") ? jsondata["link"].ToString() : null;
                            var BirthDay = Utils.DoesPropertyExist(jsondata, "birthday") ? jsondata["birthday"].ToString() : null;
                            Result.ProviderKey = Result.FacebookId != null ? Result.FacebookId.ToString() : null;

                            Result.GenderId = null;
                            if (Utils.DoesPropertyExist(jsondata, "gender"))
                            {
                                string Gender = jsondata["gender"].ToString();

                                if(Gender.ToLower()=="male")
                                {
                                    Result.GenderId = CommonsConst.Genders.Male; 
                                }
                                else if (Gender.ToLower() == "female")
                                {
                                    Result.GenderId = CommonsConst.Genders.Female;
                                }
                            }

                            if (Utils.DoesPropertyExist(jsondata, "location"))
                            {
                                try
                                {
                                    var location = jsondata["location"];
                                    if (location != null && Utils.DoesPropertyExist(location, "name") != null)
                                    {
                                        string Location = location["name"].ToString();
                                        string[] tabLocation = Location.Trim().ToLower().Split(',');
                                        //    Result.CountryCode = location.location.country_code.ToString();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Commons.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Get Location, Token =" + Token);
                                }
                            }

                        }
                    }
                }
            
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Token =" + Token);
            }
            return Result;
        }

    }
}
