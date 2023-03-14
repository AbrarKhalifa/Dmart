using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dmart.Models
{
    public class displayOrderInfo
    {

        // user table
        public int u_id { get; set; }
        public string u_username { get; set; }

        // product table
        public int pro_id { get; set; }
        public string pro_name { get; set; }
        public string pro_description { get; set; }
        public Nullable<int> pro_price { get; set; }
        public string pro_image { get; set; }

        // inv detal
        public int o_id { get; set; }
        public Nullable<int> o_fk_pro_id { get; set; }
        public Nullable<int> total_bill { get; set; }
        public Nullable<System.DateTime> order_date { get; set; }
        public Nullable<int> qty { get; set; }
        public Nullable<int> price { get; set; }
        public Nullable<int> inv_fk_id { get; set; }


        //inv
        public int inv_id { get; set; }
        public Nullable<int> inv_fk_user_id { get; set; }
        public Nullable<System.DateTime> invoice_date { get; set; }
        public string payment_method { get; set; }


    }
}