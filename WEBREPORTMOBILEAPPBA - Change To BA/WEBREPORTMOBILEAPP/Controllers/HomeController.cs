using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
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
        DBMOBILEAPPBAEntities db = new DBMOBILEAPPBAEntities();
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
            ViewBag.sku = db.tbl_sku.ToList();           
            ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
            ViewBag.nganhh = db.tbl_NganhHang.ToList();
            ViewBag.nhanh = db.tbl_NhanHang.ToList();
            var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            return View(check);

        }
        [HttpPost]
        public ActionResult Index(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            ViewBag.sku = db.tbl_sku.ToList();
            ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
            ViewBag.nganhh = db.tbl_NganhHang.ToList();
            ViewBag.nhanh = db.tbl_NhanHang.ToList();
            var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            return View(check);

        }    
        public ActionResult Login()
        {
            if (Session["user"] == null)
            {
                return View();
            }
            else if ((Session["User"] as tbl_user).Us_Role.ToLower() == "unilever")
            {
                return RedirectToAction("ViewDoiThuCanhTranh");
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
            var check = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
            return View(check);
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
            var check = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
            return View(check);
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
            var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            return View(check);
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
            var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            return View(check);
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
            var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var sku = db.tbl_sku.ToList();
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
            
            var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            //LẤY SỐ LƯỢNG BÁN CỦA TỪNG NHÂN VIÊN
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            // SỐ LƯỢNG SHOP
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var sku = db.tbl_sku.ToList();
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

            //DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            //DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            //ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            //ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");

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
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");

            //DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            //DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            //ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            //ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
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
        public ActionResult ViewDoiThuCanhTranhDG()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var kq = db.VW_doithucanhtranhdongian.Where(n => n.DTCTIMAGE_Date >= dt_bd && n.DTCTIMAGE_Date <= dt_kt).ToList();
            return View(kq);
        }
        [HttpPost]
        public ActionResult ViewDoiThuCanhTranhDG(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }


            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");

            //DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            //DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            //ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            //ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var kq = db.VW_doithucanhtranhdongian.Where(n => n.DTCTIMAGE_Date >= dt_bd && n.DTCTIMAGE_Date <= dt_kt).ToList();
            return View(kq);
        }
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
        public ActionResult ViewReportMonthly()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var banhang = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_deleteFlag==null && n.BillDetail_deleteFlag==null && n.BillDetail_deleteFlag==null).ToList();
            var checkuser = banhang.Select(n => n.Us_id).Distinct();
            var user = db.tbl_user.Where(n => checkuser.Contains(n.Us_id)).ToList();
            var kh = db.VW_KHTIEPCAN.Where(n => n.KHTC_Datereport >= dt_bd && n.KHTC_Datereport <= dt_kt && n.KHTC_DeleteFlag==null).ToList();
            var target = db.tbl_Target.Where(n => n.Target_DayStart <= dt_bd && n.Target_DayEnd >= dt_bd && n.Target_DeleteFlag==null).ToList();
            ViewBag.user = user;
            ViewBag.target = target;
            ViewBag.kh = kh;
            var lshop = banhang.Select(n => n.Shop_id).Distinct().ToList();
            ViewBag.shop = db.tbl_shop.Where(n => lshop.Contains(n.Shop_id)).ToList();
            var bigplan = db.tbl_bigplan.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt && n.Bigplan_deleteFlag==null).ToList();
            ViewBag.bigplan = bigplan;
            return View(banhang);
        }
        [HttpPost]
        public ActionResult ViewReportMonthly(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var banhang = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_deleteFlag == null && n.BillDetail_deleteFlag == null && n.BillDetail_deleteFlag == null).ToList();
            var checkuser = banhang.Select(n => n.Us_id).Distinct();
            var user = db.tbl_user.Where(n => checkuser.Contains(n.Us_id)).ToList();
            var kh = db.VW_KHTIEPCAN.Where(n => n.KHTC_Datereport >= dt_bd && n.KHTC_Datereport <= dt_kt && n.KHTC_DeleteFlag == null).ToList();
            var target = db.tbl_Target.Where(n => n.Target_DayStart <= dt_bd && n.Target_DayEnd >= dt_bd && n.Target_DeleteFlag == null).ToList();
            ViewBag.user = user;
            ViewBag.target = target;
            ViewBag.kh = kh;
            var lshop = banhang.Select(n => n.Shop_id).Distinct().ToList();
            ViewBag.shop = db.tbl_shop.Where(n => lshop.Contains(n.Shop_id)).ToList();
            var bigplan = db.tbl_bigplan.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt && n.Bigplan_deleteFlag == null).ToList();
            ViewBag.bigplan = bigplan;
            return View(banhang);
        }
        public ActionResult ViewEditBill()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var bill = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_deleteFlag == null).ToList();
            var vw_banhang = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_deleteFlag == null).ToList();
            ViewBag.banhang = vw_banhang;
            return View(bill);
        }
        [HttpPost]
        public ActionResult ViewEditBill(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var bill = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_deleteFlag == null).ToList();
            var vw_banhang = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Bill_deleteFlag == null).ToList();
            ViewBag.banhang = vw_banhang;
            return View(bill);
        }
        public ActionResult ViewDetailBill(string id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            var banhang = db.VW_banhang.Where(n => n.Bill_id == id && n.BillDetail_deleteFlag == null).ToList();
            var doiqua = db.tbl_DoiQua.Where(n => n.Bill_id == id).ToList();
            ViewBag.doiqua = doiqua;
            ViewBag.billid = id;
            return View(banhang);
        }
        [HttpPost]
        public ActionResult ActionDeleteBill(string id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            var checkbill = db.tbl_bill.SingleOrDefault(n => n.Bill_id == id);
            if (checkbill == null)
            {
                return Json(new { status = 0, message = "Vui lòng kiểm tra lại Lựa chọn của bạn! Không tìm thấy Bill ID nào có dữ liệu là " + id });
            }
            else
            {
                db.Database.ExecuteSqlCommand("exec Sp_xoabill @id", new SqlParameter("@id", id));
                return Json(new { status = 1, message = "Bạn đã xóa thành công bill có bill id là:" + id });
            }
        }
        public ActionResult EditBill(string id)
        {
            var tbl_bill = db.tbl_bill.SingleOrDefault(n => n.Bill_id == id);
            if (tbl_bill != null)
            {
                return View(tbl_bill);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }
        [HttpPost]
        public ActionResult EditBillAC(FormCollection fr)
        {
            string id = fr.Get("Txt_id");
            string dt = fr.Get("Txt_tg");
            DateTime dt_check = Convert.ToDateTime(dt);
            db.Database.ExecuteSqlCommand("exec Sp_Suabill @id,@tg", new SqlParameter("@id", id), new SqlParameter("@tg", dt));
            return Json(new { status = 1, message = "Bạn đã sửa thành công bill có bill id là:" + id });
        }
        [HttpPost]
        public ActionResult AddSPBilldetail(FormCollection fr)
        {
            string id = fr.Get("Txt_id");
            double barcode = double.Parse(fr.Get("Txt_barcode"));
            int soluong = int.Parse(fr.Get("Txt_sl"));
            int dongia = int.Parse(fr.Get("Txt_dongia"));
            db.Database.ExecuteSqlCommand("exec Sp_ThemspBillDetail @id,@bc,@sl,@dg", new SqlParameter("@id", id), new SqlParameter("@bc", barcode), new SqlParameter("@sl", soluong), new SqlParameter("@dg", dongia));
            return Json(new { status = 1, message = "Bạn đã thêm thành công sản phẩm vào đơn hàng" });
        }
        [HttpPost]
        public ActionResult DeleteSpBillDetail(string id, double ba)
        {
            var bd = db.tbl_billDetail.SingleOrDefault(n => n.Bill_id == id && n.sku_barcode == ba);
            if (bd == null)
            {
                return Json(new { status = 0, message = "Có lỗi kết nối server" });
            }
            else
            {
                db.Database.ExecuteSqlCommand("exec Sp_XoaspBillDetail @id,@ba", new SqlParameter("@id", id), new SqlParameter("@ba", ba));
                return Json(new { status = 1, message = "Bạn đã xóa thành công sản phẩm khỏi đơn hàng" });
            }

        }
        public ActionResult EditBillDetail(string id, double ba)
        {
            var detail = db.tbl_billDetail.SingleOrDefault(n => n.Bill_id == id && n.sku_barcode == ba && n.BillDetail_deleteFlag==null);
            return View(detail);
        }
        [HttpPost]
        public ActionResult EditBillDetailAc(FormCollection fr)
        {
            string id = fr.Get("Txt_id");
            double barcode = double.Parse(fr.Get("Txt_barcode"));
            int soluong = int.Parse(fr.Get("Txt_sl"));
            int dongia = int.Parse(fr.Get("Txt_dongia"));
            db.Database.ExecuteSqlCommand("exec Sp_ThemspBillDetail @id,@bc,@sl,@dg", new SqlParameter("@id", id), new SqlParameter("@bc", barcode), new SqlParameter("@sl", soluong), new SqlParameter("@dg", dongia));
            return Json(new { status = 1, message = "Bạn đã sửa thành công sản phẩm vào đơn hàng" });
        }
        [HttpGet]
        public ActionResult DoiQuaEdit(string id)
        {
            var doiqua = db.VW_DOIQUA.Where(n => n.Bill_id == id && n.DoiQua_DeleteFlag == null).ToList();
            ViewBag.billid = id;
            var ctdq = db.tbl_ChuongTrinhDoiQua.ToList();
            ViewBag.ctdq = ctdq;
            return View(doiqua);
        }
        public ActionResult Laydsdmqt(string id)
        {
            var dmqt_ctdq = db.tbl_DMQT_CTDQ.Where(n => n.CTDQ_ID == id).ToList();
            if (dmqt_ctdq == null)
            {
                return Json(new { status = 0, message = "Không tìm thấy Danh mục quà tặng !!!" });
            }
            else
            {
                var listdmqt = dmqt_ctdq.Select(n => n.DMQT_Barcode).ToList();
                var dmqt = db.tbl_DanhMucQuaTang.Where(n => listdmqt.Contains(n.DMQT_Barcode)).ToList();
                return Json(new { status = 1, data = dmqt, message = "Inven 1 !!!" });
            }

        }
        [HttpPost]
        public ActionResult ThemQuaVaoDoiQua(FormCollection fr)
        {

            string billid = fr.Get("txt_billid");
            double dmqtbarcode = double.Parse(fr.Get("SL_qua"));
            string ctdqid = fr.Get("SL_ctdq");
            int sl = int.Parse(fr.Get("Txt_sl"));
            if (dmqtbarcode == 0)
            {
                return Json(new { status = 0, message = "Bạn chưa chọn danh mục quà tặng" });
            }
            if (sl < 0)
            {
                return Json(new { status = 0, message = "Số lượng quà tặng phải lớn hơn 0" });
            }
            db.Database.ExecuteSqlCommand("exec Sp_Themdoiqua @billid,@dmqtbarcode,@ctdqid,@sl", new SqlParameter("billid", billid), new SqlParameter("dmqtbarcode", dmqtbarcode), new SqlParameter("ctdqid", ctdqid), new SqlParameter("sl", sl));
            return Json(new { status = 1, message = "Bạn đã sửa thành công sản phẩm vào đơn hàng" });
        }
        [HttpPost]
        public ActionResult XoaQuaTrongDoiQua(string id)
        {
            db.Database.ExecuteSqlCommand("exec Sp_XoaDoiqua @id", new SqlParameter("@id", id));
            return Json(new { status = 1, message = "Bạn đã xóa thành công quà khỏi đơn hàng" });
        }
        public ActionResult EditQuaTrongDoiQua(string id)
        {
            var doiqua = db.VW_DOIQUA.SingleOrDefault(n => n.DoiQua_ID == id);
            if (doiqua == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View(doiqua);
            }

        }
        [HttpPost]
        public ActionResult ThemBill(FormCollection fr)
        {
            string usid = fr.Get("txt_bacode");
            string shopid = fr.Get("txt_shopid");
            string da = fr.Get("txt_date");
            var checkuser = db.tbl_user.Where(n => n.Us_id.ToLower() == usid.ToLower()).ToList();
            var checkshop = db.tbl_shop.Where(n => n.Shop_id.ToLower() == shopid.ToLower()).ToList();
            if (checkuser.Count()==0)
            {
                return Json(new { status = 0, message = "User không tồn tại!" });
            }
            if (checkshop.Count() == 0)
            {
                return Json(new { status = 0, message = "Siêu thị/Cửa hàng không tồn tại!" });
            }
            db.Database.ExecuteSqlCommand("exec Sp_ThemBill @usid,@shopid,@date", new SqlParameter("@usid", usid), new SqlParameter("@shopid", shopid), new SqlParameter("@date", da));
            return Json(new { status = 1, message = "Bạn đã thêm thành công đơn hàng" });
        }
        public ActionResult ViewKhachHang()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var kh = db.VW_KHTIEPCAN.Where(n => n.KHTC_Datereport >= dt_bd && n.KHTC_Datereport <= dt_kt).ToList();
            return View(kh);
        }
        [HttpPost]
        public ActionResult ViewKhachHang(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var kh = db.VW_KHTIEPCAN.Where(n => n.KHTC_Datereport >= dt_bd && n.KHTC_Datereport <= dt_kt).ToList();
            return View(kh);
        }
        [HttpPost]
        public ActionResult XoaBaoCaoKH(string id)
        {
            db.Database.ExecuteSqlCommand("exec Sp_xoabaocaokh @id", new SqlParameter("@id", id));
            return Json(new { status = 1, message = "Bạn đã xóa thành công báo cáo khách hàng" });
        }
        public ActionResult SuaBaoCaoKH(string id)
        {
            var bc = db.VW_KHTIEPCAN.SingleOrDefault(n => n.KHTC_ID == id);
            if (bc==null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View(bc);
            }
            
        }
        [HttpPost]
        public ActionResult ThemBaoCaoKH(FormCollection fr)
        {
            string usid = fr.Get("txt_bacode");
            string shopid = fr.Get("txt_shopid");
            string date = fr.Get("txt_date");
            int tc = int.Parse(fr.Get("txt_tc"));
            int tctc = int.Parse(fr.Get("txt_tctc"));
            int dt = int.Parse(fr.Get("txt_dt"));
            DateTime datecheck = Convert.ToDateTime(date);
            var checkbigplan = db.tbl_bigplan.Where(n => n.Us_id.ToLower() == usid.ToLower() && n.Shop_id.ToLower() == shopid.ToLower() && n.Bigplan_date.Year == datecheck.Year && n.Bigplan_date.Month == datecheck.Month && n.Bigplan_date.Day == datecheck.Day).ToList();
            var checkuser = db.tbl_user.SingleOrDefault(n => n.Us_id.ToLower() == usid.ToLower());
            var checkshop = db.tbl_shop.SingleOrDefault(n => n.Shop_id.ToLower() == shopid.ToLower());
            if (checkuser==null)
            {
                return Json(new { status = 0, message = "Nhân viên không không tồn tại!" });
            }
            if (checkshop == null)
            {
                return Json(new { status = 0, message = "Shop/Siêu Thị không không tồn tại!" });
            }
            if (checkbigplan.Count()==0)
            {
                return Json(new { status = 0, message = "Nhân viên không có lịch làm việc tương ứng!" });
            }
            db.Database.ExecuteSqlCommand("exec sp_thembaocaokh @bigid,@tc,@tctc,@dt,@date", new SqlParameter("@bigid", checkbigplan.FirstOrDefault().Bigplan_id), new SqlParameter("@tc", tc), new SqlParameter("@tctc", tctc)
                , new SqlParameter("@dt", dt), new SqlParameter("@date", date));
            return Json(new { status = 1, message = "Bạn đã thêm báo cáo khách hàng thành công" });
        }
        [HttpPost]
        public ActionResult Suabaocaokhac(FormCollection fr)
        {
            string khid = fr.Get("txt_khtcid");
            string date = fr.Get("txt_date");
            int tc = int.Parse(fr.Get("txt_tc"));
            int tctc = int.Parse(fr.Get("txt_tctc"));
            int dt = int.Parse(fr.Get("txt_dt"));
            db.Database.ExecuteSqlCommand("exec sp_suabaocaokh @bigid,@tc,@tctc,@dt,@date", new SqlParameter("@bigid", khid), new SqlParameter("@tc", tc), new SqlParameter("@tctc", tctc)
                , new SqlParameter("@dt", dt), new SqlParameter("@date", date));
            return Json(new { status = 1, message = "Bạn đã sửa báo cáo khách hàng thành công" });
        }
        public ActionResult ViewChamCong()
        {

            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var bigplan = db.VW_Bigplan.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
            var usercheckinout = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
            var user = db.tbl_user.Where(n => n.Us_Role.ToLower() == "user").ToList();
            ViewBag.user = user;
            ViewBag.usercheck = usercheckinout;
            ViewBag.bigplan = bigplan;
            var calv = db.tbl_CaLamViecDetail.ToList();
            ViewBag.calv = calv;
            return View(usercheckinout);

        }
        [HttpPost]
        public ActionResult ViewChamCong(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var bigplan = db.VW_Bigplan.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
            var usercheckinout = db.VW_UsercheckInout.Where(n => n.Bigplan_date >= dt_bd && n.Bigplan_date <= dt_kt).ToList();
            var user = db.tbl_user.Where(n => n.Us_Role.ToLower() == "user").ToList();
            ViewBag.user = user;
            ViewBag.usercheck = usercheckinout;
            ViewBag.bigplan = bigplan;
            var calv = db.tbl_CaLamViecDetail.ToList();
            ViewBag.calv = calv;
            return View(usercheckinout);

        }
        public ActionResult ViewShop()
        {
            var result = db.tbl_shop.Where(n => n.Shop_deleteflag == null).ToList();
            return View(result);
        }
        // add new shop
        public ActionResult AddNewShop()
        {
            var nameSup = db.Tbl_SupName.Select(n => n.Sup_name).Distinct().ToList();
            ViewBag.nameSup = nameSup;
            List<string> loai = db.tbl_shop.Where(n => (n.Shop_deleteflag == null)).Select(n => n.Shop_LoaiCHCurrent).Distinct().ToList();
            ViewBag.loai = loai;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewShop([Bind(Include = "Shop_id,Shop_name,Shop_detail,Shop_npp,Shop_nppcode,Shop_Ngcode,Shop_LoaiCHCurrent,Shop_LoaiCHFuture,Shop_latidue,Shop_Longtidue,Shop_phone,Shop_Sup,Shop_GroupPrice,Shop_text2,Shop_text3,Shop_text4,Shop_float1,Shop_float2,Shop_float3,Shop_float4,Shop_bit,Shop_datetime1,Shop_datetime2,Shop_datetime3,Shop_datetime4,Shop_deleteflag,Shop_SyncFlag")] tbl_shop tbl_shop,FormCollection fr)
        {
            var nameSup = db.Tbl_SupName.Select(n => n.Sup_name).Distinct().ToList();
            ViewBag.nameSup = nameSup;
            List<string> loai = db.tbl_shop.Where(n => (n.Shop_deleteflag == null)).Select(n => n.Shop_LoaiCHCurrent).Distinct().ToList();
            ViewBag.loai = loai;
            if (ModelState.IsValid)
            {
                tbl_shop.Shop_LoaiCHCurrent = fr.Get("SL_loai");
                tbl_shop.Shop_Sup = fr.Get("nameSup");
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
            if (tbl_shop == null)
            {
                return HttpNotFound();

            }
            List<string> loai = db.tbl_shop.Where(n => n.Shop_deleteflag == null).Select(n => n.Shop_LoaiCHCurrent).Distinct().ToList();
            ViewBag.loai = loai;
            var nameSup = db.Tbl_SupName.Select(n => n.Sup_name).Distinct().ToList();
            ViewBag.nameSup = nameSup;
            return View(tbl_shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditShop([Bind(Include = "Shop_id,Shop_name,Shop_detail,Shop_npp,Shop_nppcode,Shop_Ngcode,Shop_LoaiCHCurrent,Shop_LoaiCHFuture,Shop_latidue,Shop_Longtidue,Shop_phone,Shop_Sup,Shop_GroupPrice,Shop_text2,Shop_text3,Shop_text4,Shop_float1,Shop_float2,Shop_float3,Shop_float4,Shop_bit,Shop_datetime1,Shop_datetime2,Shop_datetime3,Shop_datetime4,Shop_deleteflag,Shop_SyncFlag")] tbl_shop tbl_shop,FormCollection fr)
        {
            List<string> loai = db.tbl_shop.Where(n => n.Shop_deleteflag == null).Select(n => n.Shop_LoaiCHCurrent).Distinct().ToList();
            ViewBag.loai = loai;
            var nameSup = db.Tbl_SupName.Select(n => n.Sup_name).Distinct().ToList();
            ViewBag.nameSup = nameSup;
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
        public ActionResult ViewBaoCaoBanHangTheoNhanVien()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var NVDetail = check.Select(n => n.Us_name).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var sku = db.tbl_sku.ToList();
            var dxn = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            ViewBag.user = us;
            ViewBag.shop = shop;
            ViewBag.sku = sku;
            ViewBag.ddh = dxn;
            ViewBag.NVDetail = NVDetail;
            ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
            ViewBag.nganhh = db.tbl_NganhHang.ToList();
            ViewBag.nhanh = db.tbl_NhanHang.ToList();
            return View(check);
        }
        [HttpPost]
        public ActionResult ViewBaoCaoBanHangTheoNhanVien(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");

            var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
           
            //LẤY SỐ LƯỢNG BÁN CỦA TỪNG NHÂN VIÊN
            var checku = check.Select(n => n.Us_id).Distinct().ToList();
            // SỐ LƯỢNG SHOP
            var checks = check.Select(n => n.Shop_id).Distinct().ToList();
            var NVDetail = check.Select(n => n.Us_name).Distinct().ToList();
            var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
            var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
            var sku = db.tbl_sku.ToList();
            var dxn = db.tbl_bill.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt).ToList();
            ViewBag.user = us;
            ViewBag.shop = shop;
            ViewBag.sku = sku;
            ViewBag.ddh = dxn;
            ViewBag.NVDetail = NVDetail;
            ViewBag.nhomnh = db.tbl_NhomNganhHang.ToList();
            ViewBag.nganhh = db.tbl_NganhHang.ToList();
            ViewBag.nhanh = db.tbl_NhanHang.ToList();
            return View(check);
        }
        public ActionResult Logout()
        {
            Session.Remove("user");
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

    }
}