using Dmart.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dmart.Controllers
{
    public class UserController : Controller
    {
        DBDMartEntities db = new DBDMartEntities();
        // GET: User
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tbl_user user)
        {
            if (ModelState.IsValid)
            {
                var check = db.tbl_user.Where(x => x.u_username == user.u_username && x.u_password == user.u_password).SingleOrDefault();
                if(check != null)
                {
                    Session["u_id"] = check.u_id;
                    Session["u_username"] = check.u_username;
                    Session["u_image"] = check.u_image;
                    return RedirectToAction("AllCategory");
                }
                else
                {
                    ViewBag.error = "invalid username or password";
                }
            }

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(tbl_user user,HttpPostedFileBase imgfile)
        {
            var path = UploadFile(imgfile);
            if(path == null)
            {
                ViewBag.error = "image not uploaded";
            }
            else
            {
                tbl_user u = new tbl_user();
                u.u_image = path;
                u.u_username = user.u_username;
                u.u_password = user.u_password;
                u.u_email = user.u_email;
                u.u_contact = user.u_contact;
                db.tbl_user.Add(u);
                db.SaveChanges();
                Session["u_id"] = u.u_id;
                Session["u_username"] = u.u_username;
                Session["u_image"] = u.u_image;
                return RedirectToAction("AllCategory");
            }

            return View();
        }



        public ActionResult AllCategory(int? page)
        {
            if(Session["u_id"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                int pagesize = 9, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.tbl_categories.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
                IPagedList<tbl_categories> cate = list.ToPagedList(pageindex, pagesize);
                return View(cate);
            }
            
        }
        [HttpPost]
        public ActionResult AllCategory(int? page, string search)
        {
            List<tbl_categories> ct = db.tbl_categories.Where(x => x.cat_name.Contains(search)).ToList();
            if (ct == null)
            {
                int pagesize = 9, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.tbl_categories.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
                IPagedList<tbl_categories> cate = list.ToPagedList(pageindex, pagesize);
                return View(cate);
            }
            else
            {
                int pagesize = 8, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.tbl_categories.Where(x => x.cat_name.Contains(search)).OrderByDescending(x => x.cat_id).ToList();
                IPagedList<tbl_categories> cate = list.ToPagedList(pageindex, pagesize);
                return View(cate);
            }

        }

        // add product functionality
        public ActionResult addProduct()
        {
            if(Session["u_id"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                List<tbl_categories> list = db.tbl_categories.ToList();
                ViewBag.listCategory = new SelectList(list, "cat_id", "cat_name");
            }
            return View();
        }

        [HttpPost]
        public ActionResult addProduct(tbl_product pr , HttpPostedFileBase imgfile)
        {
            List<tbl_categories> list = db.tbl_categories.ToList();
            ViewBag.listCategory = new SelectList(list, "cat_id", "cat_name");

            var path = UploadFile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "image cannot uploaded";
            }
            else
            {
                tbl_product p = new tbl_product();
                p.pro_image = path;
                p.pro_name = pr.pro_name;
                p.pro_description = pr.pro_description;
                p.pro_price = pr.pro_price;
                p.cat_id_fk = pr.pro_user_id_fk;
                p.pro_user_id_fk = Convert.ToInt32(Session["u_id"].ToString());
                db.tbl_product.Add(p);
                db.SaveChanges();
                ViewBag.success = "product added successfully";
                
                        
            }
            return View();
        }

        // viewproduct functionality

        public ActionResult viewProduct(int? id , int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_product.Where(x =>x.cat_id_fk == id).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<tbl_product> cate = list.ToPagedList(pageindex, pagesize);
            return View(cate);
        }
    
        // view product description
        public ActionResult viewProductDesc(int? id)
        {
            tbl_product pt = new tbl_product();

            tbl_product pr = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();
            pt.pro_id = pr.pro_id;
            pt.pro_name = pr.pro_name;
            pt.pro_price = pr.pro_price;
            pt.pro_description = pr.pro_description;
            pt.pro_image = pr.pro_image;



            return View(pt);
        }


        // ad to cart
        public ActionResult addToCart(int? id)
        {
            tbl_product pr = db.tbl_product.Where(x => x.pro_id == id).FirstOrDefault();
            return View(pr);
        }
        [HttpPost]
        public ActionResult addToCart(int pid,int qty,int price)
        {
            int uid = Convert.ToInt32(Session["u_id"].ToString());
            tbl_cart ct = db.tbl_cart.Where(x => x.c_fk_user_id == uid && x.c_fk_pro_id == pid).SingleOrDefault();
           
            if(ct == null)
            {
                tbl_cart c = new tbl_cart();
                c.c_qty = qty;
                c.c_price = price;
                c.c_fk_pro_id = pid;
                c.c_fk_user_id = uid;
                db.tbl_cart.Add(c);
                db.SaveChanges();
                return RedirectToAction("AllCategory");
            }
            else
            {
                ct.c_qty += qty;
                db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AllCategory");
            }

            ViewBag.error = "not adding into cart";
            return View();
        }

        // display cart item
        public ActionResult viewCartDetails()
        {
            int uid = Convert.ToInt32(Session["u_id"].ToString());
            tbl_user us = db.tbl_user.Find(uid);

            List<cartDisplay> li = new List<cartDisplay>();

            if(us != null)
            {
                List<tbl_cart> ct = db.tbl_cart.Where(x => x.c_fk_user_id == uid).ToList();
                int totalAll = 0;
                foreach(var item in ct)
                {
                    tbl_product pr = db.tbl_product.Where(x => x.pro_id == item.c_fk_pro_id).SingleOrDefault();
                    cartDisplay cd = new cartDisplay();
                    cd.pro_id = Convert.ToInt32(item.c_fk_pro_id);
                    cd.pro_image = pr.pro_image;
                    cd.pro_name = pr.pro_name;
                    cd.pro_price = pr.pro_price;
                    cd.c_qty = item.c_qty;
                    cd.total = cd.pro_price * item.c_qty;
                    totalAll += cd.total.Value;
                    li.Add(cd);
                }
                ViewBag.totalAll = totalAll;
            }

            return View(li);
        }

        // removeCart

        public ActionResult removeCart(int id)
        {

            var ids = db.tbl_cart.Where(x => x.c_fk_pro_id == id).Single();
            db.tbl_cart.Remove(ids);
            db.SaveChanges();
           

            return RedirectToAction("viewCartDetails");
        }
        // profile

        public ActionResult profile()
        {
            int uid = Convert.ToInt32(Session["u_id"].ToString());
            var user = db.tbl_user.Find(uid);
            return View(user);
        }
        [HttpPost]

        public ActionResult profile(tbl_user us)
        {
            tbl_user a = db.tbl_user.Where(x => x.u_id == us.u_id).FirstOrDefault();
            a.u_username = us.u_username;
            a.u_password = us.u_password;
            a.u_email = us.u_email;
            a.u_contact = us.u_contact;
            db.Entry(a).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AllCategory");
        }

        public ActionResult signout()
        {

            Session.RemoveAll();
            return RedirectToAction("Login");
        }

        // order
        public ActionResult order()
        {
            int uid = Convert.ToInt32(Session["u_id"].ToString());
            tbl_user us = db.tbl_user.Find(uid);

            List<displayOrderInfo> li = new List<displayOrderInfo>();

            if(us != null)
            {
                List<tbl_invoice> ct = db.tbl_invoice.Where(x => x.inv_fk_user_id == uid).ToList();
                int totalAll = 0;

                foreach(var item in ct)
                {
                    List<tbl_invoiceDetail> ind = db.tbl_invoiceDetail.Where(x => x.inv_fk_id == item.inv_id).ToList();

                    foreach(var it in ind)
                    {
                        tbl_product pr = db.tbl_product.Where(x => x.pro_id == it.o_fk_pro_id).SingleOrDefault();

                        // custom class
                        displayOrderInfo cd = new displayOrderInfo();
                        cd.o_id = it.o_id;
                        cd.inv_id = item.inv_id;
                        cd.order_date = item.invoice_date;
                        cd.pro_id = Convert.ToInt32(it.o_fk_pro_id);
                        cd.pro_image = pr.pro_image;
                        cd.pro_name = pr.pro_name;
                        cd.price = it.price;
                        cd.qty = it.qty;
                        cd.total_bill = it.total_bill;
                        totalAll += cd.total_bill.Value;

                        li.Add(cd);
                    }
                }

                ViewBag.totalAll = totalAll;
                 return View(li);
            }

            return View();
        }



        // upload image code
        public string UploadFile(HttpPostedFileBase file)
        {
            Random r = new Random();
            String path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {

                string extentions = Path.GetExtension(file.FileName);
                if (extentions.ToLower().Equals(".jpg") || extentions.ToLower().Equals(".jpeg") || extentions.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }

                }
                else
                {
                    Response.Write("<script>alert('Only jpg, jpeg, or png formats are acceptable');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file');</script>");
                path = "-1";
            }

            return path;
        }





    }
}