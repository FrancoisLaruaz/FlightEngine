using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Configuration;

namespace CommonsConst
{
    public static class Const
    {


        public const string DefaultCulture = "en";
        public const string WebsiteTitle = "Website Title";
        public const string GoogleAnalyticsID = "";
        public const string FacebookPixelID = "110600362985483";
        public const string EMailDev = "francois.laruaz@gmail.com";

        // Define if the application need  login/logout features
        public const bool UserManagement = true;

        public const int MaxImageLength = 10000000;

        public const string BasePathRessources = "~/Ressources";
        public const string BasePathUpload = BasePathRessources + "/Upload";
        public const string BasePathUploadDecrypted = BasePathUpload + "/Decrypted";
        public const string BasePathUploadEncrypted = BasePathUpload + "/Encrypted";
        public const string BasePathFiles = BasePathRessources + "/Files";
        public const string BasePathTemplateEMails = BasePathRessources + "\\EMails";
        public const string BasePathImages = BasePathFiles + "/Images";
        public const string BasePathHomePage = BasePathImages + "/HomePage";
        public const string BasePathUploadThumbnail = BasePathUploadDecrypted + "/Thumbnail";

        public const string Logo =BasePathImages + "/Logo.png";

        public const string BaseMetaKeyWords = "";

        public const string i18nlangtag = "i18n.langtag";

        public const string UserSession = "UserSession";
        public const string LastConnectionDate = "LastConnectionDate";
        public const string WebcamCaptureSession = "WebcamCaptureSession";
        public const string JsonConstantsSession = "JsonConstantsSession";
        public const string DefaultUserLocalization = "DefaultUserLocalization";
        public const string WebsiteLanguageSession = "WebsiteLanguageSession";

        public const string ColorWebsite = "rgba(45,191,183, 1)";
        public const string ColorWebsiteHexa = "#2DBFB7";

        public const string DefaultSelect = "--- [[[Select]]] ---";

        public const string DefaultCurrency = "EUR";

        public const decimal DefaultGoogleMapSearchDistance =5;
    }

    public static class MonthsOfYear
    {
        public static Dictionary<string, int> MonthOptions = new Dictionary<string, int>()
    {
        {"[[[January]]]", 1 },
        {"[[[February]]]", 2 },
        {"[[[March]]]", 3 },
        {"[[[April]]]", 4 },
        {"[[[May]]]", 5 },
        {"[[[June]]]", 6 },
        {"[[[July]]]", 7 },
        {"[[[August]]]", 8 },
        {"[[[September]]]", 9 },
        {"[[[October]]]", 10 },
        {"[[[November]]]", 11 },
        {"[[[December]]]", 12 }
    };

    }


    public static class MyProfileShow
    {
        public const string Profile = "Profile";
        public const string Settings = "Settings";
    }

    public static class SliderHomePage
    {
        public const string Image1 = Const.BasePathHomePage+ "/slider_homepage1.jpg";
        public const string Image2 = Const.BasePathHomePage + "/slider_homepage2.jpg";
        public const string Image3 = Const.BasePathHomePage + "/slider_homepage3.jpg";
        public const string Image4 = Const.BasePathHomePage + "/slider_homepage4.jpg";
    }

    public static class SearchParameters
    {
        public const int MaxDisplayedItems = 10;
    }

    public static class JavascriptCookies
    {
        public const string ComplianceAccepted = "is_cookie_compliance_accepted";
    }

    public static class BaseMetaData
    {
        public const string KeyWords =  CommonsConst.Const.WebsiteTitle+ ",KeyWord1,KeyWord2";
        public const string Title = "Title";
        public const string MetaImageSrc =  "/Ressources/Files/Images/Logo.png" ;
        public const string Description = "Description.";
    }

    public static class DefaultImage
    {
        public const string Page = Const.BasePathImages + "/Logo.png";
        public const string BackgroundPicture = Const.BasePathImages+ "/Background.jpg";
        public const string Default = Const.BasePathImages + "/DefaultImage.jpg";
        public const string DefaultImageUser = Const.BasePathImages + "/DefaultUser.jpg";
        public const string DefaultThumbnailUser = Const.BasePathImages + "/DefaultThumbnailUser.jpg";
        public const string Empty = Const.BasePathImages + "/Email/collect.gif";
    }
    public static class SearchItemType
    {
        public const string User = "[[[User]]]";
        public const string Page = "[[[Page]]]";
        public const string SearchAll = "[[[SearchAll]]]";
    }

    public static class PartialViewResults
    {
        public static string NotAuthorized = "NotAuthorized";
        public static string UnknownError = "UnknownError";
    }

    public static class EmailWatcherStatus
    {
        public const int NotWatched = 10001;
        public const int EmailNotOpened = 10002;
        public const int LinkClicked = 10003;
        public const int EmailOpened = 10004;

        public static string ToString(int value)
        {
            switch (value)
            {
                case NotWatched:
                    return "[[[Not Watched]]]";
                case EmailNotOpened:
                    return "[[[Email Not Opened]]]";
                case LinkClicked:
                    return "[[[Link Clicked]]]";
                case EmailOpened:
                    return "[[[Email Opened]]]";
                default:
                    return "";
            }
        }
    }


    public static class FileSize
    {
        public const int LightPicture = 300000;
        public const int HeavyPicture = 1000000;
        public const int Document = 10000000;

        public static string ToString(int sizeInByte)
        {
            switch (sizeInByte)
            {
                case LightPicture:
                    return "[[[300 KB]]]";
                case HeavyPicture:
                    return "[[[1 MB]]]";
                case Document:
                    return "[[[10 MB]]]";
                default:
                    return string.Format("{0} bytes", sizeInByte);
            }
        }
    }

    public static class ErrorMessages
    {
        public static string NotAuthorized = "[[[You are not authorized to perform this action.]]]";
        public static string UnknownError = "[[[Unknown retrieval error.]]]";
        public static string UploadError = "[[[Error while uploading the file.]]]";
    }

    public static class SearchTypes
    {
        public static string User = "[[[User]]]";
        public static string Page = "[[[Page]]]";
    }

    public static class SuccessMessages
    {
        public static string Success = "[[[The action has been successfully performed.]]]";
    }

    public static class EmailTypes
    {
        public const int Forgotpassword = 1001;
        public const int UserWelcome = 1002;
        public const int News = 1003;
        public const int ResetPassword = 1004;
        public const int Contact = 1005;
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
    }

    public static class ExternalAuthentificationRedirection
    {
        public const string RedirectToEmailSignUp = "RedirectToEmailSignUp";
        public const string RedirectToLogin = "RedirectToLogin";
        public const string RedirectToExternalSignUp = "RedirectToExternalSignUp";
    }

    public static class CategoryTypes
    {
        public const int EmailType = 1;
        public const int UserRole = 2;
        public const int Language = 3;
        public const int TaskLogType = 4;
        public const int NewsType = 5;
        public const int TypeUserMailing = 6;
        public const int Gender = 7;
        public const int ProductStatus = 8;
    }

    public static class LoginProviders
    {
        public static string Facebook = "Facebook";
        public static string Google = "Google";
    }

    public static class TimeFormat
    {
        public const string Month_Day_Year_Time_12h = "MMM dd, yyyy hh:mm tt"; //eg. Mar 10, 2018 11:00 AM
        public const string Month_Day_Year = "MMM dd, yyyy"; //eg. Apr 04, 2018
        public const string Month_Year = "MMM yyyy"; //eg. Apr 2018
        public const string Day_Month_Time_24h = "dd MMM HH:mm"; //eg. `28 Mar 19:17`
    }

    public static class ProductStatus
    {
        public const int Active = 8001;
        public const int Disabled = 8002;

        public static string ToString(int value)
        {
            switch (value)
            {
                case Active:
                    return "Active";
                case Disabled:
                    return "Disabled";
                default:
                    return "";
            }
        }

    }

    public static class Languages
    {
        public const int English = 3001;
        public const int French = 3002;

        public static string ToString(int value)
        {
            switch (value)
            {
                case English:
                    return "en";
                case French:
                    return "fr";
                default:
                    return "";
            }
        }

        public static int ToInt(string value)
        {
            switch (value)
            {
                case "en":
                    return English;
                case "fr":
                    return French;
                default:
                    return ToInt(Const.DefaultCulture);
            }
        }
    }


    public static class Periodicities
    {
        public const string Daily = "[[[Daily]]]";
    }

    public static class ScheduledTaskTypes
    {
        public const string SendEMailToUser = "SendEMailToUser";
        public const string SendNews = "SendNews";
    }

    public static class TaskLogTypes
    {
        public const string DeleteLogs = "DeleteLogs";
        public const string DeleteUploadedFile = "DeleteUploadedFile";
        public const string ConvertCurrency = "ConvertCurrency";
    }

    public static class TypeUserMailing
    {
        public const int AllUsers = 6001;
        public const int ConfirmedUsers = 6002;
    }


    public static class Genders
    {
        public const int Female = 7001;
        public const int Male = 7002;
        public const int DoNotWantToAnswer = 7003;
    }

    public static class NewsType
    {
        public const int PublishOnly = 5001;
        public const int PublishAndMail = 5002;
        public const int MailOnly = 5003;
    }


    public static class StaticFileVersion
    {
        public const string StaticFileVersionString = "20180206";
    }

}
