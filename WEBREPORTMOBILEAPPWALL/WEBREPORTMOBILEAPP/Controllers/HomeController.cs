using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WEBREPORTMOBILEAPP.Models;

namespace WEBREPORTMOBILEAPP.Controllers
{
    public class HomeController : Controller
    {
        DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();
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
            var supbigplan = db.tbl_SupBigPlan.Where(n => n.SupBigPlan_date >= dt_bd && n.SupBigPlan_date <= dt_kt).ToList();
            return View();
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
            return View();
        }
    }
}