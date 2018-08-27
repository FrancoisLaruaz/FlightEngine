using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Shared
{
    public class HeaderViewModel
    {
        public string UserFirstName { get; set; }

        public string UserNameDecrypt { get; set; }

        public string PictureThumbnailSrc { get; set; }

        public HeaderViewModel()
        {
            UserNameDecrypt = "User Profile";
        }
    }
}
