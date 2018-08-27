using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models.ViewModels.Account
{
    public class MyProfilePhotosViewModel
    {
        public int UserId { get; set; }

        public string PictureSrc { get; set; }

        public string PictureThumbnailSrc { get; set; }

        public string PictureDecryptSrc { get; set; }


        public MyProfilePhotosViewModel()
        {
            
        }
    }
}
