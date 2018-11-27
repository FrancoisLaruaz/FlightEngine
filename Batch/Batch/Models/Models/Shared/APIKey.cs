using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Shared
{
        public class APIKey
        {
            public string Key { get; set; }

            public int RequestsNumber { get; set; }



            public APIKey()
            {
                RequestsNumber = 0;
            }

            public APIKey(string _Key)
            {
                RequestsNumber = 0;
                Key = _Key;
            }

        }

}
