using Dmart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dmart.Controllers
{
    public class CheckoutController : Controller
    {
        DBDMartEntities db = new DBDMartEntities();
        // GET: Checkout
        public ActionResult checkout()
        {
            int uid = Convert.ToInt32(Session["u_id"].ToString());
            List<tbl_cart> li = db.tbl_cart.Where(x => x.c_fk_user_id == uid).ToList();

            tbl_invoice iv = new tbl_invoice();
            iv.inv_fk_user_id = uid;
            iv.invoice_date = System.DateTime.Now;
            iv.total_bill = 0;
            iv.payment_method = "cash";
            db.tbl_invoice.Add(iv);

            int total = 0;

            foreach(var item in li)
            {
                tbl_invoiceDetail od = new tbl_invoiceDetail();
                od.o_fk_pro_id = item.c_fk_pro_id;
                od.order_date = System.DateTime.Now;
                od.qty = item.c_qty;
                od.price = item.tbl_product.pro_price;
                od.total_bill = item.c_qty * item.tbl_product.pro_price;
                iv.tbl_invoiceDetail.Add(od);
                total += od.total_bill.Value;

                var deleteCart = db.tbl_cart.Find(item.c_id);
                db.tbl_cart.Remove(deleteCart);
                db.SaveChanges();
            }

            iv.total_bill = total;
            db.SaveChanges();


            return RedirectToAction("AllCategory","User");
        }
    }
}