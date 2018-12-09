using FlightsEngine.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models.Transavia
{


    public class TransaviaRoutesResponse
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string id { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public Self2 self { get; set; }
    }

    public class Origin
    {
        public string id { get; set; }
        public Self self { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Destination
    {
        public string id { get; set; }
        public Self1 self { get; set; }
    }

    public class Self1
    {
        public string href { get; set; }
    }

    public class Self2
    {
        public string href { get; set; }
    }




}