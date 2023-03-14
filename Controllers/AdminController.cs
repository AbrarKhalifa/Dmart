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
    public class AdminController : Controller
    {
        DBDMartEntities db = new DBDMartEntities();
        // GET: User
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(tbl_admin user)
        {
            if (ModelState.IsValid)
            {
                var check = db.tbl_admin.Where(x => x.ad_username == user.ad_username && x.ad_password == user.ad_password).SingleOrDefault();
                if (check != null)
                {
                    Session["ad_id"] = check.ad_id;
                    Session["ad_username"] = check.ad_username;
                    Session["ad_image"] = check.ad_image;
                    return RedirectToAction("AllCategory");
                }
                else
                {
                    ViewBag.error = "invalid username or password";
                }
            }

            return View();
        }
        public ActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAdmin(tbl_admin user, HttpPostedFileBase imgfile)
        {
            var path = UploadFile(imgfile);
            if (path == null)
            {
                ViewBag.error = "image not uploaded";
            }
            else
            {
                tbl_admin u = new tbl_admin();
                u.ad_image = path;
                u.ad_username = user.ad_username;
                u.ad_password = user.ad_password;
               
                db.tbl_admin.Add(u);
                db.SaveChanges();
                Session["ad_id"] = u.ad_id;
                Session["ad_username"] = u.ad_username;
                Session["ad_image"] = u.ad_image;
                return RedirectToAction("AllCategory");
            }

            return View();
        }



        public ActionResult AllCategory(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_categories.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_categories> cate = list.ToPagedList(pageindex, pagesize);
            return View(cate);
        }
        [HttpPost]
        public ActionResult AllCategory(int? page , string search)
        {
            List<tbl_categories> ct = db.tbl_categories.Where(x => x.cat_name.Contains(search)).ToList();
            if(ct == null)
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

        public ActionResult AddCategory()
        {
            return View();
        
        }
        [HttpPost]
        public ActionResult AddCategory(tbl_categories cat, HttpPostedFileBase imgfile)
        {
            var path = UploadFile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "image not uploded";
            }
            else
            {

                tbl_categories c = new tbl_categories();
                c.cat_image = path;
                c.cat_name = cat.cat_name;
                c.cat_status = 1;
                c.ad_id_fk = Convert.ToInt32(Session["ad_id"].ToString());
                db.tbl_categories.Add(c);
                db.SaveChanges();
                ViewBag.success = "category added successfully";


            }
            return View();

        }


        public ActionResult Delete(int id)
        {
            var ids = db.tbl_categories.Find(id);
            db.tbl_categories.Remove(ids);
            db.SaveChanges();

            return RedirectToAction("AllCategory");
        }

        public ActionResult signout()
        {

            Session.RemoveAll();
            return RedirectToAction("LoginAdmin");
        }


        // viewproduct functionality

        public ActionResult viewProduct(int? id, int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_product.Where(x => x.cat_id_fk == id).OrderByDescending(x => x.pro_id).ToList();
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


        // viewAllProduct

        public ActionResult viewAllProducts(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_product.OrderByDescending(x => x.pro_id).ToList();
            IPagedList<tbl_product> cate = list.ToPagedList(pageindex, pagesize);
            return View(cate);
        }
        [HttpPost]
        public ActionResult viewAllProducts(int? page, string search)
        {
            List<tbl_product> ct = db.tbl_product.Where(x => x.pro_name.Contains(search)).ToList();
            if (ct == null)
            {
                int pagesize = 9, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.tbl_product.OrderByDescending(x => x.pro_id).ToList();
                IPagedList<tbl_product> cate = list.ToPagedList(pageindex, pagesize);
                return View(cate);
            }
            else
            {
                int pagesize = 8, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.tbl_product.Where(x => x.pro_name.Contains(search)).OrderByDescending(x => x.pro_id).ToList();
                IPagedList<tbl_product> cate = list.ToPagedList(pageindex, pagesize);
                return View(cate);
            }

        }

        // profile

        public ActionResult profile()
        {
            int uid = Convert.ToInt32(Session["ad_id"].ToString());
            var user = db.tbl_admin.Find(uid);
            return View(user);
        }
        [HttpPost]

        public ActionResult profile(tbl_admin us)
        {
            tbl_admin a = db.tbl_admin.Where(x => x.ad_id == us.ad_id).FirstOrDefault();
            a.ad_username = us.ad_username;
            a.ad_password = us.ad_password;
            db.Entry(a).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AllCategory");
        }

        //user list

        public ActionResult ViewAllUsers()
        {
            List<tbl_user> li = db.tbl_user.ToList();
            return View(li);
        }


        // order info 
        public ActionResult OrderAdmin()
        {
            int uid = Convert.ToInt32(Session["ad_id"].ToString());
            var user = db.tbl_admin.Find(uid);

            List<displayOrderInfo> li = new List<displayOrderInfo>();

            if(user != null)
            {
                List<tbl_invoiceDetail> inv = db.tbl_invoiceDetail.ToList();
                int totalAll = 0;

                foreach(var item in inv)
                {
                    displayOrderInfo ds = new displayOrderInfo();
                    ds.o_id = item.o_id;
                    ds.order_date = item.order_date;
                    ds.price = item.price;
                    ds.qty = item.qty;
                    ds.inv_id = (int)item.inv_fk_id;
                    ds.total_bill = item.price * item.qty;
                    ds.pro_name = item.tbl_product.pro_name;
                    ds.pro_image = item.tbl_product.pro_image;
                    totalAll += ds.total_bill.Value;
                    li.Add(ds);
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