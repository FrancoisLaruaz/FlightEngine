using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.ExternalAuthentification
{
    public class ExternalAuthentificationResult
    {
        public ExternalAuthentificationResult(bool success, string returnUrl, string error, string media, string imageSrc, bool isSignUp, string language,  bool _IsAlreadyLoggedIn, string redirection = null, string firstName = null, string lastName = null)
        {
            Success = success;
            ReturnUrl = returnUrl;
            Error = error;
            Media = media;
            IsSignUp = isSignUp;
            ImageSrc = imageSrc;
            Language = language;
            Redirection = redirection;
            FirstName = firstName;
            LastName = lastName;
            IsAlreadyLoggedIn = _IsAlreadyLoggedIn;
        }

        public bool IsAlreadyLoggedIn { get; set; }
        public bool Success { get; set; }
        public string ReturnUrl { get; set; }

        public string ImageSrc { get; set; }

        public string Media { get; set; }

        public string Error { get; set; }

        public bool IsSignUp { get; set; }

        public string Redirection { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Language { get; set; }
    }
}
