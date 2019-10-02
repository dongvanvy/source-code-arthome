using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WEBREPORTMOBILEAPP.Models;

namespace WEBREPORTMOBILEAPP.Controllers
{
    public class HomeController : Controller
    {
        DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
            ViewBag.nganhh = db.tbl_NganhHang.ToList();
            ViewBag.nhanh = db.tbl_NhanHang.ToList();
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).ToList();
                var checkkq = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                var skulist = checkkq.Select(n => n.sku_barcode).Distinct().ToList();
                ViewBag.sku = db.tbl_sku.Where(n=>skulist.Contains(n.sku_barcode)).ToList();
                return View(checkkq);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var checkkq = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id).ToList();
                var skulist = checkkq.Select(n => n.sku_barcode).Distinct().ToList();
                ViewBag.sku = db.tbl_sku.Where(n => skulist.Contains(n.sku_barcode)).ToList();
                return View(checkkq);
            }
            else
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                var skulist = check.Select(n => n.sku_barcode).Distinct().ToList();
                ViewBag.sku = db.tbl_sku.Where(n => skulist.Contains(n.sku_barcode)).ToList();
                return View(check);
            }

        }
        [HttpPost]
        public ActionResult Index(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            var user = Session["user"] as tbl_user;
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
            ViewBag.nganhh = db.tbl_NganhHang.ToList();
            ViewBag.nhanh = db.tbl_NhanHang.ToList();
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).ToList();
                var checkkq = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                var skulist = checkkq.Select(n => n.sku_barcode).Distinct().ToList();
                ViewBag.sku = db.tbl_sku.Where(n => skulist.Contains(n.sku_barcode)).ToList();
                return View(checkkq);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var checkkq = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id).ToList();
                var skulist = checkkq.Select(n => n.sku_barcode).Distinct().ToList();
                ViewBag.sku = db.tbl_sku.Where(n => skulist.Contains(n.sku_barcode)).ToList();
                return View(checkkq);
            }
            else
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                var skulist = check.Select(n => n.sku_barcode).Distinct().ToList();
                ViewBag.sku = db.tbl_sku.Where(n => skulist.Contains(n.sku_barcode)).ToList();
                return View(check);
            }
        }
        public ActionResult Login()
        {
            if (Session["user"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult LoginAc(FormCollection fr)
        {
            Session["user"] = null;
            string username = fr.Get("txt_username").ToString();
            string password = fr.Get("txt_password").ToString();
            string passmahoa = GetMD5(password);
            var ktuser = db.tbl_user.SingleOrDefault(n => n.Us_username == username && n.Us_password == passmahoa);
            if (ktuser == null)
            {
                return Json(new { status = 0, message = "Username hoặc Mật Khẩu không đúng!\n Vui lòng kiểm tra lại!\n" });
            }
            else if (ktuser.Us_bit == false)
            {
                return Json(new { status = 0, message = "Username của bạn đã bị quản trị viên vô hiệu hóa!\n" });
            }
            else
            {
                Session["User"] = ktuser;
                Session["role"] = ktuser.Us_Role;
                return Json(new { status = 1, message = "Xin chào " + ktuser.Us_name + "\n" });
            }
        }
        public static string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();
        }
        [HttpGet]
        public ActionResult ViewCheckInOut()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.Contains(user.Us_id)).Select(n => n.Us_id).ToList();
                var checkkq = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                return View(checkkq);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var checkkq = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt && n.Us_id==user.Us_id).ToList();
                return View(checkkq);
            }
            else
            {
                var check = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
                return View(check);
            }
            
        }
        [HttpPost]
        public ActionResult ViewCheckInOut(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.Contains(user.Us_id)).Select(n => n.Us_id).ToList();
                var checkkq = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                return View(checkkq);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var checkkq = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt && n.Us_id == user.Us_id).ToList();
                return View(checkkq);
            }
            else
            {
                var check = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
                return View(check);
            }
        }
        [HttpGet]
        public ActionResult ViewNhomNganhHang()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).ToList();
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                return View(check);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id==user.Us_id).ToList();
                return View(check);
            }
            else
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                return View(check);
            }
        }
        [HttpPost]
        public ActionResult ViewNhomNganhHang(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }

            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).ToList();
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                return View(check);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id).ToList();
                return View(check);
            }
            else
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                return View(check);
            }
        }
        [HttpGet]
        public ActionResult ViewBaoCaoHinhAnh()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var check = db.VW_BCHINHANH.Where(n => n.BCHA_Time >= dt_bd && n.BCHA_Time <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var big = db.tbl_bigplan.Where(n => checku.Contains(n.Us_id) && checks.Contains(n.Shop_id) && n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
            ViewBag.big = big;
            ViewBag.user = us;
            ViewBag.shop = shop;
            return View(check);
        }
        [HttpPost]
        public ActionResult ViewBaoCaoHinhAnh(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var check = db.VW_BCHINHANH.Where(n => n.BCHA_Time >= dt_bd && n.BCHA_Time <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var big = db.tbl_bigplan.Where(n => checku.Contains(n.Us_id) && checks.Contains(n.Shop_id) && n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
            ViewBag.big = big;
            ViewBag.user = us;
            ViewBag.shop = shop;
            return View(check);
        }
        [HttpGet]
        public ActionResult ViewBaoCaoDeXuatNhapHang()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var check = db.VW_DeXuatNhapHang.Where(n => n.DDH_time >= dt_bd && n.DDH_time <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var sku = db.tbl_sku.ToList();
            var dxn = db.tbl_Dondathang.Where(n => n.DDH_time >= dt_bd && n.DDH_time <= dt_kt).ToList();
            ViewBag.user = us;
            ViewBag.shop = shop;
            ViewBag.sku = sku;
            ViewBag.ddh = dxn;
            return View(check);
        }
        [HttpPost]
        public ActionResult ViewBaoCaoDeXuatNhapHang(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var check = db.VW_DeXuatNhapHang.Where(n => n.DDH_time >= dt_bd && n.DDH_time <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var dxn = db.tbl_Dondathang.Where(n => n.DDH_time >= dt_bd && n.DDH_time <= dt_kt).ToList();
            var sku = db.tbl_sku.ToList();
            ViewBag.user = us;
            ViewBag.shop = shop;
            ViewBag.sku = sku;
            ViewBag.ddh = dxn;
            return View(check);
        }
        [HttpGet]
        public ActionResult ViewBaoCaoNhapHang()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var check = db.VW_NHAPHANG.Where(n => n.DNH_Date >= dt_bd && n.DNH_Date <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var sku = db.tbl_sku.ToList();
            var dxn = db.Tbl_DonNhapHang.Where(n => n.DNH_Date >= dt_bd && n.DNH_Date <= dt_kt).ToList();
            ViewBag.user = us;
            ViewBag.shop = shop;
            ViewBag.sku = sku;
            ViewBag.ddh = dxn;
            return View(check);
        }
        [HttpPost]
        public ActionResult ViewBaoCaoNhapHang(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var check = db.VW_NHAPHANG.Where(n => n.DNH_Date >= dt_bd && n.DNH_Date <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var sku = db.tbl_sku.ToList();
            var dxn = db.Tbl_DonNhapHang.Where(n => n.DNH_Date >= dt_bd && n.DNH_Date <= dt_kt).ToList();
            ViewBag.user = us;
            ViewBag.shop = shop;
            ViewBag.sku = sku;
            ViewBag.ddh = dxn;
            return View(check);
        }
        [HttpGet]
        public ActionResult ViewBaoCaoBanHangTheoCH()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).Distinct().ToList();
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checksku = check.Select(n => n.sku_barcode).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var sku = db.tbl_sku.Where(n=>checksku.Contains(n.sku_barcode)).ToList();
                var dxn = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && checku.Contains(n.Us_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.sku = sku;
                ViewBag.ddh = dxn;
                ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
                ViewBag.nganhh = db.tbl_NganhHang.ToList();
                ViewBag.nhanh = db.tbl_NhanHang.ToList();
                return View(check);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id==user.Us_id).ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checksku = check.Select(n => n.sku_barcode).Distinct().ToList();
                var us = db.tbl_user.Where(n => n.Us_id==user.Us_id).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var sku = db.tbl_sku.Where(n => checksku.Contains(n.sku_barcode)).ToList();
                var dxn = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id==user.Us_id).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.sku = sku;
                ViewBag.ddh = dxn;
                ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
                ViewBag.nganhh = db.tbl_NganhHang.ToList();
                ViewBag.nhanh = db.tbl_NhanHang.ToList();
                return View(check);
            }
            else
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup == user.Us_id).Select(n => n.Us_id).Distinct().ToList();
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checksku = check.Select(n => n.sku_barcode).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var sku = db.tbl_sku.Where(n => checksku.Contains(n.sku_barcode)).ToList();
                var dxn = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.sku = sku;
                ViewBag.ddh = dxn;
                ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
                ViewBag.nganhh = db.tbl_NganhHang.ToList();
                ViewBag.nhanh = db.tbl_NhanHang.ToList();
                return View(check);
            }
        }
        [HttpPost]
        public ActionResult ViewBaoCaoBanHangTheoCH(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).Distinct().ToList();
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checksku = check.Select(n => n.sku_barcode).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var sku = db.tbl_sku.Where(n => checksku.Contains(n.sku_barcode)).ToList();
                var dxn = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && checku.Contains(n.Us_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.sku = sku;
                ViewBag.ddh = dxn;
                ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
                ViewBag.nganhh = db.tbl_NganhHang.ToList();
                ViewBag.nhanh = db.tbl_NhanHang.ToList();
                return View(check);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id).ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checksku = check.Select(n => n.sku_barcode).Distinct().ToList();
                var us = db.tbl_user.Where(n => n.Us_id == user.Us_id).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var sku = db.tbl_sku.Where(n => checksku.Contains(n.sku_barcode)).ToList();
                var dxn = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.sku = sku;
                ViewBag.ddh = dxn;
                ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
                ViewBag.nganhh = db.tbl_NganhHang.ToList();
                ViewBag.nhanh = db.tbl_NhanHang.ToList();
                return View(check);
            }
            else
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup == user.Us_id).Select(n => n.Us_id).Distinct().ToList();
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checksku = check.Select(n => n.sku_barcode).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var sku = db.tbl_sku.Where(n => checksku.Contains(n.sku_barcode)).ToList();
                var dxn = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.sku = sku;
                ViewBag.ddh = dxn;
                ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
                ViewBag.nganhh = db.tbl_NganhHang.ToList();
                ViewBag.nhanh = db.tbl_NhanHang.ToList();
                return View(check);
            }
        }
        [HttpGet]
        public ActionResult ViewBaoCaoCheckInSup()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var check = db.VW_SupCheckin.Where(n => n.SupBigPlan_date >= dt_bd && n.SupBigPlan_date <= dt_kt).ToList();
            return View(check);
        }
        [HttpPost]
        public ActionResult ViewBaoCaoCheckInSup(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var check = db.VW_SupCheckin.Where(n => n.SupBigPlan_date >= dt_bd && n.SupBigPlan_date <= dt_kt).ToList();
            return View(check);
        }
        [HttpGet]
        public ActionResult ViewBaoCaoSupChamDiem()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var check = db.VW_Supchamdiem.Where(n => n.SupBigPlan_date >= dt_bd && n.SupBigPlan_date <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var sup = db.Tbl_SupName.Where(n => n.Sup_Status == true).ToList();
            var bigplan = db.tbl_SupBigPlan.Where(n => n.SupBigPlan_date >= dt_bd && n.SupBigPlan_date <= dt_kt).ToList();
            ViewBag.bigplan = bigplan;
            ViewBag.user = us;
            ViewBag.shop = shop;
            ViewBag.sup = sup;
            return View(check);
        }
        [HttpPost]
        public ActionResult ViewBaoCaoSupChamDiem(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var check = db.VW_Supchamdiem.Where(n => n.SupBigPlan_date >= dt_bd && n.SupBigPlan_date <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var sup = db.Tbl_SupName.Where(n => n.Sup_Status == true).ToList();
            var bigplan = db.tbl_SupBigPlan.Where(n => n.SupBigPlan_date >= dt_bd && n.SupBigPlan_date <= dt_kt).ToList();
            ViewBag.bigplan = bigplan;
            ViewBag.user = us;
            ViewBag.shop = shop;
            ViewBag.sup = sup;
            return View(check);
        }
        [HttpGet]
        public ActionResult ViewBaoCaoDoiQua()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
           
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).ToList();
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                return View(check);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id==user.Us_id).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => n.Us_id==user.Us_id).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                return View(check);
            }
            else
            {
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                return View(check);
            }
        }
        [HttpPost]
        public ActionResult ViewBaoCaoDoiQua(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).ToList();
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                return View(check);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => n.Us_id == user.Us_id).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                return View(check);
            }
            else
            {
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                return View(check);
            }
        }
        [HttpGet]
        public ActionResult ViewBaoCaoChiTietDoiQua()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).ToList();
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checkb = check.Select(n => n.Bill_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var bill = db.tbl_bill.Where(n => checkb.Contains(n.Bill_id)).ToList();
                var banhang = db.VW_banhang.Where(n => checkb.Contains(n.Bill_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                ViewBag.banhang = banhang;
                ViewBag.bill = bill;
                return View(check);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id==user.Us_id).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checkb = check.Select(n => n.Bill_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => n.Us_id==user.Us_id).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var bill = db.tbl_bill.Where(n => checkb.Contains(n.Bill_id)).ToList();
                var banhang = db.VW_banhang.Where(n => checkb.Contains(n.Bill_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                ViewBag.banhang = banhang;
                ViewBag.bill = bill;
                return View(check);
            }
            else
            {
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checkb = check.Select(n => n.Bill_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var bill = db.tbl_bill.Where(n => checkb.Contains(n.Bill_id)).ToList();
                var banhang = db.VW_banhang.Where(n => checkb.Contains(n.Bill_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                ViewBag.banhang = banhang;
                ViewBag.bill = bill;
                return View(check);
            }
        }
        [HttpPost]
        public ActionResult ViewBaoCaoChiTietDoiQua(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
                var usercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).Select(n => n.Us_id).ToList();
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && usercheck.Contains(n.Us_id)).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checkb = check.Select(n => n.Bill_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var bill = db.tbl_bill.Where(n => checkb.Contains(n.Bill_id)).ToList();
                var banhang = db.VW_banhang.Where(n => checkb.Contains(n.Bill_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                ViewBag.banhang = banhang;
                ViewBag.bill = bill;
                return View(check);
            }
            else if (user.Us_Role.ToLower() == "user")
            {
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checkb = check.Select(n => n.Bill_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => n.Us_id == user.Us_id).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var bill = db.tbl_bill.Where(n => checkb.Contains(n.Bill_id)).ToList();
                var banhang = db.VW_banhang.Where(n => checkb.Contains(n.Bill_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                ViewBag.banhang = banhang;
                ViewBag.bill = bill;
                return View(check);
            }
            else
            {
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => n.DMQT_Status == true).ToList();
                var check = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var checkb = check.Select(n => n.Bill_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var bill = db.tbl_bill.Where(n => checkb.Contains(n.Bill_id)).ToList();
                var banhang = db.VW_banhang.Where(n => checkb.Contains(n.Bill_id)).ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.dmqt = dmqt;
                ViewBag.banhang = banhang;
                ViewBag.bill = bill;
                return View(check);
            }

        }
        [HttpGet]
        public ActionResult ViewBaoCaoLLVSup()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var sup = db.Tbl_SupName.Where(n => n.Sup_Status == true).ToList();
            var supbigplan = db.tbl_SupBigPlan.Where(n => n.SupBigPlan_date >= dt_bd && n.SupBigPlan_date <= dt_kt).ToList();
            var checksup = supbigplan.Select(n => n.SupBigPlan_id).ToList();
            var checkin = db.tbl_SupCheckIn.Where(n => checksup.Contains(n.SupBigPlan_id)).ToList();
            ViewBag.check = checkin;
            ViewBag.sup = sup;
            return View(supbigplan);
        }
        [HttpPost]
        public ActionResult ViewBaoCaoLLVSup(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var sup = db.Tbl_SupName.Where(n => n.Sup_Status == true).ToList();
            var supbigplan = db.tbl_SupBigPlan.Where(n => n.SupBigPlan_date >= dt_bd && n.SupBigPlan_date <= dt_kt).ToList();
            var checksup = supbigplan.Select(n => n.SupBigPlan_id).ToList();
            var checkin = db.tbl_SupCheckIn.Where(n => checksup.Contains(n.SupBigPlan_id)).ToList();
            ViewBag.check = checkin;
            ViewBag.sup = sup;
            return View(supbigplan);
        }
        public ActionResult ViewDoiThuCanhTranh()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.cty = db.tbl_DTCTCty.ToList();
            ViewBag.brand = db.tbl_DTCTBrand.ToList();
            ViewBag.format = db.tbl_DTCTformat.ToList();
            ViewBag.variant = db.tbl_DTCTVARIANT.ToList();
            ViewBag.crm = db.tbl_DTCTcrmrange.ToList();
            ViewBag.ff = db.tbl_DTCTffrange.ToList();
            ViewBag.onoff = db.tbl_DTCTonpostoffpost.ToList();
            ViewBag.type = db.tbl_DTCTtype.ToList();
            ViewBag.ltn = db.tbl_DTCTltmrange.ToList();
            var kq = db.tbl_doithucanhtranh.Where(n => n.dt_thoigianbd >= dt_bd && n.dt_thoigianbd <= dt_kt).ToList();
            return View(kq);
        }
        [HttpPost]
        public ActionResult ViewDoiThuCanhTranh(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.cty = db.tbl_DTCTCty.ToList();
            ViewBag.brand = db.tbl_DTCTBrand.ToList();
            ViewBag.format = db.tbl_DTCTformat.ToList();
            ViewBag.variant = db.tbl_DTCTVARIANT.ToList();
            ViewBag.crm = db.tbl_DTCTcrmrange.ToList();
            ViewBag.ff = db.tbl_DTCTffrange.ToList();
            ViewBag.onoff = db.tbl_DTCTonpostoffpost.ToList();
            ViewBag.type = db.tbl_DTCTtype.ToList();
            ViewBag.ltn = db.tbl_DTCTltmrange.ToList();
            var kq = db.tbl_doithucanhtranh.Where(n => n.dt_thoigianbd >= dt_bd && n.dt_thoigianbd <= dt_kt).ToList();
            return View(kq);
        }
        //public ActionResult ViewDoiThuCanhTranhDG()
        //{
        //    if (Session["user"] == null)
        //    {
        //        return RedirectToAction("login");
        //    }
        //    DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
        //    DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
        //    ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
        //    ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
        //    var kq = db.vw.Where(n => n.DTCTIMAGE_Date >= dt_bd && n.DTCTIMAGE_Date <= dt_kt).ToList();
        //    return View(kq);
        //}
        [HttpPost]
        //public ActionResult ViewDoiThuCanhTranhDG(FormCollection fr)
        //{
        //    if (Session["user"] == null)
        //    {
        //        return RedirectToAction("login");
        //    }
        //    DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
        //    DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
        //    ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
        //    ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
        //    var kq = db.VW_doithucanhtranhdongian.Where(n => n.DTCTIMAGE_Date >= dt_bd && n.DTCTIMAGE_Date <= dt_kt).ToList();
        //    return View(kq);
        //}
        public ActionResult ViewShowLocation(string id)
        {
            var usercheckinout = db.tbl_UserCheckInOut.SingleOrDefault(n => n.UserCheckInOut_ID == id);
            var bigplan = db.tbl_bigplan.SingleOrDefault(n => n.Bigplan_id == usercheckinout.Bigplan_id);
            var shop = db.tbl_shop.SingleOrDefault(n => n.Shop_id == bigplan.Shop_id);
            ViewBag.big = bigplan;
            ViewBag.shop = shop;
            ViewBag.uch = usercheckinout;

            string markers = "[";
            markers += "{";
            markers += string.Format("'title': '{0}',", bigplan.Us_id);
            markers += string.Format("'lat': '{0}',", usercheckinout.UserCheckInOut_LatidueCI);
            markers += string.Format("'lng': '{0}',", usercheckinout.UserCheckInOut_LongtidueCI);
            markers += string.Format("'description': '{0}'", bigplan.Us_id);
            markers += "},";
            markers += "];";
            ViewBag.Markers = markers;
            return View();
        }
        public ActionResult ViewShowLocationCkout(string id)
        {
            var usercheckinout = db.tbl_UserCheckInOut.SingleOrDefault(n => n.UserCheckInOut_ID == id);
            var bigplan = db.tbl_bigplan.SingleOrDefault(n => n.Bigplan_id == usercheckinout.Bigplan_id);
            var shop = db.tbl_shop.SingleOrDefault(n => n.Shop_id == bigplan.Shop_id);
            ViewBag.big = bigplan;
            ViewBag.shop = shop;
            ViewBag.uch = usercheckinout;

            string markers = "[";
            markers += "{";
            markers += string.Format("'title': '{0}',", bigplan.Us_id);
            markers += string.Format("'lat': '{0}',", usercheckinout.UserCheckInOut_LatidueCI);
            markers += string.Format("'lng': '{0}',", usercheckinout.UserCheckInOut_LongtidueCI);
            markers += string.Format("'description': '{0}'", bigplan.Us_id);
            markers += "},";
            markers += "];";
            ViewBag.Markers = markers;
            return View();
        }
        public ActionResult ViewShowLocationSup(string id)
        {
            var supcheck = db.tbl_SupCheckIn.SingleOrDefault(n => n.SupCheckIn_id == id);
            var bigplan = db.tbl_SupBigPlan.SingleOrDefault(n => n.SupBigPlan_id == supcheck.SupBigPlan_id);
            var shop = db.tbl_shop.SingleOrDefault(n => n.Shop_id == bigplan.Shop_id);
            ViewBag.big = bigplan;
            ViewBag.shop = shop;
            ViewBag.uch = supcheck;

            string markers = "[";
            markers += "{";
            markers += string.Format("'title': '{0}',", bigplan.Us_id);
            markers += string.Format("'lat': '{0}',", supcheck.SupCheckIn_Latidue);
            markers += string.Format("'lng': '{0}',", supcheck.SupCheckIn_Longtidue);
            markers += string.Format("'description': '{0}'", bigplan.Us_id);
            markers += "},";
            markers += "];";
            ViewBag.Markers = markers;
            return View();
        }
        public ActionResult ViewShowLocationCkoutSup(string id)
        {
            var supcheck = db.tbl_SupCheckIn.SingleOrDefault(n => n.SupCheckIn_id == id);
            var bigplan = db.tbl_SupBigPlan.SingleOrDefault(n => n.SupBigPlan_id == supcheck.SupBigPlan_id);
            var shop = db.tbl_shop.SingleOrDefault(n => n.Shop_id == bigplan.Shop_id);
            ViewBag.big = bigplan;
            ViewBag.shop = shop;
            ViewBag.uch = supcheck;

            string markers = "[";
            markers += "{";
            markers += string.Format("'title': '{0}',", bigplan.Us_id);
            markers += string.Format("'lat': '{0}',", supcheck.SupCheckIn_Latidue);
            markers += string.Format("'lng': '{0}',", supcheck.SupCheckIn_Longtidue);
            markers += string.Format("'description': '{0}'", bigplan.Us_id);
            markers += "},";
            markers += "];";
            ViewBag.Markers = markers;
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult ViewReportGift()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            int hbd = 1;
            int hkt = 23;
            ViewBag.ca = "0";
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower()=="user")
            {
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var listbillid = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                
                var lchuongtrinhdoiqua = doiqua.Select(n => n.CTDQ_ID).Distinct().ToList();
                var dmqtctdq = db.tbl_DMQT_CTDQ.ToList();
                var lquatang = dmqtctdq.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).Select(n => n.DMQT_Barcode).Distinct().ToList();
                var quatang = db.tbl_DanhMucQuaTang.Where(n => lquatang.Contains(n.DMQT_Barcode)).ToList();
                var chuongtrinhdoiqua = db.tbl_ChuongTrinhDoiQua.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).ToList();
                var Us = db.tbl_user.Where(n => n.Us_id==user.Us_id).ToList();
                var bill = db.tbl_bill.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.user = Us;
                ViewBag.doiqua = doiqua;
                ViewBag.chuongtrinhdoiqua = chuongtrinhdoiqua;
                ViewBag.quatang = quatang;
                ViewBag.dmqt = dmqtctdq;
                ViewBag.bill = bill;
                var sms = db.VW_SMSOTP.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.sms = sms;
                return View(banhang);
            }
            else if (user.Us_Role.ToLower()=="sup")
            {
                var lusercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).ToList();
                var lusid = lusercheck.Select(n => n.Us_id).ToList();
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && lusid.Contains(n.Us_id) && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var listbillid = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                var ludoiqua = doiqua.Select(n => n.Us_id).Distinct().ToList();
                var lchuongtrinhdoiqua = doiqua.Select(n => n.CTDQ_ID).Distinct().ToList();
                var dmqtctdq = db.tbl_DMQT_CTDQ.ToList();
                var lquatang = dmqtctdq.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).Select(n => n.DMQT_Barcode).Distinct().ToList();
                var quatang = db.tbl_DanhMucQuaTang.Where(n => lquatang.Contains(n.DMQT_Barcode)).ToList();
                var chuongtrinhdoiqua = db.tbl_ChuongTrinhDoiQua.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).ToList();
                var Us = lusercheck;
                var bill = db.tbl_bill.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.user = Us;
                ViewBag.doiqua = doiqua;
                ViewBag.chuongtrinhdoiqua = chuongtrinhdoiqua;
                ViewBag.quatang = quatang;
                ViewBag.dmqt = dmqtctdq;
                ViewBag.bill = bill;
                var sms = db.VW_SMSOTP.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.sms = sms;
                return View(banhang);
            }
            else
            {
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var listbillid = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n=>listbillid.Contains(n.Bill_id)).ToList();
                var ludoiqua = doiqua.Select(n => n.Us_id).Distinct().ToList();
                var lchuongtrinhdoiqua = doiqua.Select(n => n.CTDQ_ID).Distinct().ToList();
                var dmqtctdq = db.tbl_DMQT_CTDQ.ToList();
                var lquatang = dmqtctdq.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).Select(n => n.DMQT_Barcode).Distinct().ToList();
                var quatang = db.tbl_DanhMucQuaTang.Where(n => lquatang.Contains(n.DMQT_Barcode)).ToList();
                var chuongtrinhdoiqua = db.tbl_ChuongTrinhDoiQua.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).ToList();
                var Us = db.tbl_user.Where(n => ludoiqua.Contains(n.Us_id)).Distinct().ToList();
                var bill = db.tbl_bill.Where(n=>listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.user = Us;
                ViewBag.doiqua = doiqua;
                ViewBag.chuongtrinhdoiqua = chuongtrinhdoiqua;
                ViewBag.quatang = quatang;
                ViewBag.dmqt = dmqtctdq;
                ViewBag.bill = bill;
                var sms = db.VW_SMSOTP.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.sms = sms;
                return View(banhang);
            }
        }
        [HttpPost]
        public ActionResult ViewReportGift(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            int hbd;
            int hkt;
            if (fr.Get("txt_ca").ToString()=="1")
            {
                hbd = 1;
                hkt = 11;
                ViewBag.ca = "1";
            }
            else if (fr.Get("txt_ca").ToString() == "2")
            {
                hbd = 12;
                hkt = 23;
                ViewBag.ca = "2";
            }
            else
            {
                hbd = 1;
                hkt = 23;
                ViewBag.ca = "0";
            }
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "user")
            {
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var listbillid = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n => listbillid.Contains(n.Bill_id)).ToList();

                var lchuongtrinhdoiqua = doiqua.Select(n => n.CTDQ_ID).Distinct().ToList();
                var dmqtctdq = db.tbl_DMQT_CTDQ.ToList();
                var lquatang = dmqtctdq.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).Select(n => n.DMQT_Barcode).Distinct().ToList();
                var quatang = db.tbl_DanhMucQuaTang.Where(n => lquatang.Contains(n.DMQT_Barcode)).ToList();
                var chuongtrinhdoiqua = db.tbl_ChuongTrinhDoiQua.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).ToList();
                var Us = db.tbl_user.Where(n => n.Us_id == user.Us_id).ToList();
                var bill = db.tbl_bill.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.user = Us;
                ViewBag.doiqua = doiqua;
                ViewBag.chuongtrinhdoiqua = chuongtrinhdoiqua;
                ViewBag.quatang = quatang;
                ViewBag.dmqt = dmqtctdq;
                ViewBag.bill = bill;
                var sms = db.VW_SMSOTP.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.sms = sms;
                return View(banhang);
            }
            else if (user.Us_Role.ToLower() == "sup")
            {
                var lusercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).ToList();
                var lusid = lusercheck.Select(n => n.Us_id).ToList();
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && lusid.Contains(n.Us_id) && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var listbillid = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                var ludoiqua = doiqua.Select(n => n.Us_id).Distinct().ToList();
                var lchuongtrinhdoiqua = doiqua.Select(n => n.CTDQ_ID).Distinct().ToList();
                var dmqtctdq = db.tbl_DMQT_CTDQ.ToList();
                var lquatang = dmqtctdq.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).Select(n => n.DMQT_Barcode).Distinct().ToList();
                var quatang = db.tbl_DanhMucQuaTang.Where(n => lquatang.Contains(n.DMQT_Barcode)).ToList();
                var chuongtrinhdoiqua = db.tbl_ChuongTrinhDoiQua.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).ToList();
                var Us = lusercheck;
                var bill = db.tbl_bill.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.user = Us;
                ViewBag.doiqua = doiqua;
                ViewBag.chuongtrinhdoiqua = chuongtrinhdoiqua;
                ViewBag.quatang = quatang;
                ViewBag.dmqt = dmqtctdq;
                ViewBag.bill = bill;
                var sms = db.VW_SMSOTP.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.sms = sms;
                return View(banhang);
            }
            else
            {
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var listbillid = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                var ludoiqua = doiqua.Select(n => n.Us_id).Distinct().ToList();
                var lchuongtrinhdoiqua = doiqua.Select(n => n.CTDQ_ID).Distinct().ToList();
                var dmqtctdq = db.tbl_DMQT_CTDQ.ToList();
                var lquatang = dmqtctdq.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).Select(n => n.DMQT_Barcode).Distinct().ToList();
                var quatang = db.tbl_DanhMucQuaTang.Where(n => lquatang.Contains(n.DMQT_Barcode)).ToList();
                var chuongtrinhdoiqua = db.tbl_ChuongTrinhDoiQua.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).ToList();
                var Us = db.tbl_user.Where(n => ludoiqua.Contains(n.Us_id)).Distinct().ToList();
                var bill = db.tbl_bill.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.user = Us;
                ViewBag.doiqua = doiqua;
                ViewBag.chuongtrinhdoiqua = chuongtrinhdoiqua;
                ViewBag.quatang = quatang;
                ViewBag.dmqt = dmqtctdq;
                ViewBag.bill = bill;
                var sms = db.VW_SMSOTP.Where(n => listbillid.Contains(n.Bill_id)).ToList();
                ViewBag.sms = sms;
                return View(banhang);
            }
        }
        [HttpGet]
        public ActionResult ViewReportComfort()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var user = Session["user"] as tbl_user;
            var banhang = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            var ludoiqua = doiqua.Select(n => n.Us_id).Distinct().ToList();
            var lchuongtrinhdoiqua = doiqua.Select(n => n.CTDQ_ID).Distinct().ToList();
            var dmqtctdq = db.tbl_DMQT_CTDQ.ToList();
            var lquatang = dmqtctdq.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).Select(n => n.DMQT_Barcode).Distinct().ToList();
            var quatang = db.tbl_DanhMucQuaTang.Where(n => lquatang.Contains(n.DMQT_Barcode)).ToList();
            var chuongtrinhdoiqua = db.tbl_ChuongTrinhDoiQua.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).ToList();
            var Us = db.tbl_user.Where(n => ludoiqua.Contains(n.Us_id)).Distinct().ToList();
            var bill = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            ViewBag.user = Us;
            ViewBag.doiqua = doiqua;
            ViewBag.chuongtrinhdoiqua = chuongtrinhdoiqua;
            ViewBag.quatang = quatang;
            ViewBag.dmqt = dmqtctdq;
            ViewBag.bill = bill;
            return View(banhang);

        }
        [HttpPost]
        public ActionResult ViewReportComfort(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var user = Session["user"] as tbl_user;
            var banhang = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            var ludoiqua = doiqua.Select(n => n.Us_id).Distinct().ToList();
            var lchuongtrinhdoiqua = doiqua.Select(n => n.CTDQ_ID).Distinct().ToList();
            var dmqtctdq = db.tbl_DMQT_CTDQ.ToList();
            var lquatang = dmqtctdq.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).Select(n => n.DMQT_Barcode).Distinct().ToList();
            var quatang = db.tbl_DanhMucQuaTang.Where(n => lquatang.Contains(n.DMQT_Barcode)).ToList();
            var chuongtrinhdoiqua = db.tbl_ChuongTrinhDoiQua.Where(n => lchuongtrinhdoiqua.Contains(n.CTDQ_ID)).ToList();
            var Us = db.tbl_user.Where(n => ludoiqua.Contains(n.Us_id)).Distinct().ToList();
            ViewBag.user = Us;
            ViewBag.doiqua = doiqua;
            ViewBag.chuongtrinhdoiqua = chuongtrinhdoiqua;
            ViewBag.quatang = quatang;
            ViewBag.dmqt = dmqtctdq;
            var bill = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            ViewBag.bill = bill;
            return View(banhang);

        }
        public ActionResult ViewReportSku()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            int hbd = 1;
            int hkt = 23;
            ViewBag.ca = "0";
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "user")
            {
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var lbill = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n =>lbill.Contains(n.Bill_id)).ToList();
                var Us = db.tbl_user.Where(n => n.Us_id == user.Us_id).ToList();
                var lsku = banhang.Select(n => n.sku_barcode).Distinct().ToList();
                var sku = db.tbl_sku.Where(n => lsku.Contains(n.sku_barcode)).ToList();
                ViewBag.sku = sku;
                ViewBag.user = Us;
                return View(banhang);
            }
            else if (user.Us_Role.ToLower() == "sup")
            {
                var lusercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).ToList();
                var lusid = lusercheck.Select(n => n.Us_id).ToList();
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && lusid.Contains(n.Us_id) && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var lbill = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n =>lbill.Contains(n.Bill_id)).ToList();
                var Us = lusercheck;
                var lsku = banhang.Select(n => n.sku_barcode).Distinct().ToList();
                var sku = db.tbl_sku.Where(n => lsku.Contains(n.sku_barcode)).ToList();
                ViewBag.sku = sku;
                ViewBag.user = Us;
                ViewBag.doiqua = doiqua;
                return View(banhang);
            }
            else
            {
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var lbill = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n => lbill.Contains(n.Bill_id)).ToList();
                var Us = db.tbl_user.Where(n => n.Us_Role.ToLower() == "user").ToList();
                var lsku = banhang.Select(n => n.sku_barcode).Distinct().ToList();
                var sku = db.tbl_sku.Where(n => lsku.Contains(n.sku_barcode)).ToList();
                ViewBag.sku = sku;
                ViewBag.doiqua = doiqua;
                ViewBag.user = Us;
                return View(banhang);
            }
        }
        [HttpPost]
        public ActionResult ViewReportSku(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            int hbd;
            int hkt;
            if (fr.Get("txt_ca").ToString() == "1")
            {
                hbd = 1;
                hkt = 11;
                ViewBag.ca = "1";
            }
            else if (fr.Get("txt_ca").ToString() == "2")
            {
                hbd = 12;
                hkt = 23;
                ViewBag.ca = "2";
            }
            else
            {
                hbd = 1;
                hkt = 23;
                ViewBag.ca = "0";
            }
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "user")
            {
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Us_id == user.Us_id && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var lbill = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n => lbill.Contains(n.Bill_id)).ToList();
                var Us = db.tbl_user.Where(n => n.Us_id == user.Us_id).ToList();
                var lsku = banhang.Select(n => n.sku_barcode).Distinct().ToList();
                var sku = db.tbl_sku.Where(n => lsku.Contains(n.sku_barcode)).ToList();
                ViewBag.sku = sku;
                ViewBag.user = Us;
                return View(banhang);
            }
            else if (user.Us_Role.ToLower() == "sup")
            {
                var lusercheck = db.tbl_user.Where(n => n.Us_SupGroup.ToLower().Contains(user.Us_id.ToLower())).ToList();
                var lusid = lusercheck.Select(n => n.Us_id).ToList();
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && lusid.Contains(n.Us_id) && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt).ToList();
                var lbill = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n => lbill.Contains(n.Bill_id)).ToList();
                var Us = lusercheck;
                var lsku = banhang.Select(n => n.sku_barcode).Distinct().ToList();
                var sku = db.tbl_sku.Where(n => lsku.Contains(n.sku_barcode)).ToList();
                ViewBag.sku = sku;
                ViewBag.user = Us;
                ViewBag.doiqua = doiqua;
                return View(banhang);
            }
            else
            {
                var Us = db.tbl_user.Where(n => n.Us_Role.ToLower() == "user").ToList();
                var luser = Us.Select(n => n.Us_id).Distinct().ToList();
                var doiqua = db.VW_DOIQUA.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_date.Value.Hour >= hbd && n.Bill_date.Value.Hour <= hkt && luser.Contains(n.Us_id)).ToList();
                var lbill = doiqua.Select(n => n.Bill_id).Distinct().ToList();
                var banhang = db.VW_banhang.Where(n => lbill.Contains(n.Bill_id)).ToList();
                var lsku = banhang.Select(n => n.sku_barcode).Distinct().ToList();
                var sku = db.tbl_sku.Where(n => lsku.Contains(n.sku_barcode)).ToList();
                ViewBag.sku = sku;
                ViewBag.doiqua = doiqua;
                ViewBag.user = Us;
                return View(banhang);
            }
        }


        //-------> tbl_ Shop
        public ActionResult ViewShop()
        {
            var result = db.tbl_shop.ToList();
            return View(result);
        }
        // add new shop
        public ActionResult AddNewShop()
        {
            var loai = db.tbl_shop.Where(n => n.Shop_deleteflag == null).Select(n => n.Shop_LoaiCHCurrent).Distinct().ToList();
            ViewBag.loai = loai;
            var nameSup = db.Tbl_SupName.Select(n => n.Sup_name).Distinct().ToList();
            ViewBag.nameSup = nameSup;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewShop([Bind(Include = "Shop_id,Shop_name,Shop_detail,Shop_npp,Shop_nppcode,Shop_Ngcode,Shop_LoaiCHCurrent,Shop_LoaiCHFuture,Shop_latidue,Shop_Longtidue,Shop_phone,Shop_Sup,Shop_GroupPrice,Shop_text2,Shop_text3,Shop_text4,Shop_float1,Shop_float2,Shop_float3,Shop_float4,Shop_bit,Shop_datetime1,Shop_datetime2,Shop_datetime3,Shop_datetime4,Shop_deleteflag,Shop_SyncFlag")] tbl_shop tbl_shop,FormCollection fr)
        {
            var loai = db.tbl_shop.Where(n => n.Shop_deleteflag == null).Select(n => n.Shop_LoaiCHCurrent).Distinct().ToList();
            ViewBag.loai = loai;
            var nameSup = db.Tbl_SupName.Select(n => n.Sup_name).Distinct().ToList();
            ViewBag.nameSup = nameSup;
            if (ModelState.IsValid)
            {
                tbl_shop.Shop_LoaiCHCurrent = fr.Get("SL_loai");
                tbl_shop.Shop_LoaiCHCurrent = fr.Get("nameSup");
                db.tbl_shop.Add(tbl_shop);
                db.SaveChanges();
                return RedirectToAction("ViewShop");
            }

            return View(tbl_shop);
        }

        public ActionResult EditShop(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_shop tbl_shop = db.tbl_shop.Find(id);
            var loai = db.tbl_shop.Where(n => n.Shop_deleteflag == null).Select(n => n.Shop_LoaiCHCurrent).Distinct().ToList();
            ViewBag.loai = loai;
            var nameSup = db.Tbl_SupName.Select(n => n.Sup_name).Distinct().ToList();
            ViewBag.namesup = nameSup;
            if (tbl_shop == null)
            {
                return HttpNotFound();
            }
            return View(tbl_shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditShop([Bind(Include = "Shop_id,Shop_name,Shop_detail,Shop_npp,Shop_nppcode,Shop_Ngcode,Shop_LoaiCHCurrent,Shop_LoaiCHFuture,Shop_latidue,Shop_Longtidue,Shop_phone,Shop_Sup,Shop_GroupPrice,Shop_text2,Shop_text3,Shop_text4,Shop_float1,Shop_float2,Shop_float3,Shop_float4,Shop_bit,Shop_datetime1,Shop_datetime2,Shop_datetime3,Shop_datetime4,Shop_deleteflag,Shop_SyncFlag")] tbl_shop tbl_shop, FormCollection fr)
        {           
            var loai = db.tbl_shop.Where(n => n.Shop_deleteflag == null).Select(n => n.Shop_LoaiCHCurrent).Distinct().ToList();
            ViewBag.loai = loai;
            var nameSup = db.Tbl_SupName.Select(n => n.Sup_name).Distinct().ToList();
            ViewBag.namesup = nameSup;
            if (ModelState.IsValid)
            {
                tbl_shop.Shop_LoaiCHCurrent = fr.Get("SL_loai");
                tbl_shop.Shop_Sup = fr.Get("nameSup");
                db.Entry(tbl_shop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewShop");
            }
            return View(tbl_shop);
        }

        // GET: Shop/Delete/5
        public ActionResult DeleteShop(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_shop tbl_shop = db.tbl_shop.Find(id);
            if (tbl_shop == null)
            {
                return HttpNotFound();
            }
            return View(tbl_shop);
        }

        [HttpPost, ActionName("DeleteShop")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tbl_shop tbl_shop = db.tbl_shop.Find(id);
            tbl_shop.Shop_deleteflag = DateTime.Now;
            db.Entry(tbl_shop).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewShop");

            //--> remove dataabase
            //tbl_shop movie = db.tbl_shop.Find(id);
            //db.tbl_shop.Remove(movie);
            //db.SaveChanges();
            //return RedirectToAction("ViewShop");
        }

    }
}