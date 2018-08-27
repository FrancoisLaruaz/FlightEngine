using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Models.Class.FileUpload
{
    public class FileUpload
    {
        public FileUpload()
        {
            CreateThumbnail = false;
            EncryptFile = false;
            IsImage = false;
        }
        public string UploadName { get; set; }

        public bool IsImage { get; set; }

        public HttpPostedFileBase File { get; set; }
        public byte[] FileBytes { get; set; }
        public byte[] ThumbBytes { get; set; }
        public bool CreateThumbnail { get; set; }
        public bool EncryptFile { get; set; }
    }
}
