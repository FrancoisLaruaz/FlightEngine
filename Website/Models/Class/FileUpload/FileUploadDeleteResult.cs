using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.FileUpload
{

    public class FileUploadDeleteResult
    {
        public bool Result { get; set; }


        public int FilesAnalyzedNumber { get; set; }

        public int FilesDeletedNumber { get; set; }

        public int FilesErrorsNumber { get; set; }


        public string Error { get; set; }


        public FileUploadDeleteResult()
        {
            FilesAnalyzedNumber = 0;
            FilesDeletedNumber = 0;
            FilesErrorsNumber=0;
            Error = "";
        }
    }
}
