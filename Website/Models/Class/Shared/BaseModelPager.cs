using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class
{
    public class BaseModelPager
    {
        public int StartAt { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public BaseModelPager()
        {
            StartAt = 0;
            PageSize = 10;
        }
    }
}
