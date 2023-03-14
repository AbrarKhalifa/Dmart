using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dmart.Models
{
    public class cartDisplay
    {

        public int pro_id { get; set; }
        public string pro_name { get; set; }
        public string pro_description { get; set; }
        public Nullable<int> pro_price { get; set; }
        public string pro_image { get; set; }
        public Nullable<int> c_qty { get; set; }
        public Nullable<int> total { get; set; }

    }
}