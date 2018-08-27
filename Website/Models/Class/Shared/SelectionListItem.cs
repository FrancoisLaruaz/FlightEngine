using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Class
{
    public class SelectionListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public SelectionListItem()
        {

        }

        public SelectionListItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public SelectionListItem(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }
    }
}
