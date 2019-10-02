using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBREPORTMOBILEAPP.Models;

namespace WEBREPORTMOBILEAPP.Controllers
{
    public class BaoCaoController : Controller
    {
        DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();
        // GET: BaoCao   
        [HttpGet]
        public ActionResult ViewReportBanHang()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login","Home");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "supnpp")
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Shop_npp == user.Us_name.ToLower()).ToList();
                //var check = db.VW_BCHINHANH.Where(n => n.BCHA_Time >= dt_bd && n.BCHA_Time <= dt_kt && n.Shop_npp == user.Us_name.ToLower()).ToList();
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
            else
            {
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
            
        }
        [HttpPost]
        public ActionResult ViewReportBanHang(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login","Home");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "supnpp")
            {
                var check = db.VW_banhang.Where(n => n.Bill_date >= dt_bd && n.Bill_date <= dt_kt && n.Shop_npp == user.Us_name.ToLower()).ToList();
                //var check = db.VW_BCHINHANH.Where(n => n.BCHA_Time >= dt_bd && n.BCHA_Time <= dt_kt && n.Shop_npp == user.Us_name.ToLower()).ToList();
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
            else
            {
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

        }
        //BAO CAO ĐỀ XUẤT NHẬP HÀNG        
        public ActionResult ViewReportDeXuatNhapHang()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "supnpp")
            {                
                var check = db.VW_DeXuatNhapHang.Where(n => n.DDH_time >= dt_bd && n.DDH_time <= dt_kt && n.Shop_npp == user.Us_name.ToLower()).ToList();
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
              else
            {
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
          
            
        }
        [HttpPost]
        public ActionResult ViewReportDeXuatNhapHang(FormCollection fr)
        {

            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "supnpp")
            {               
                // có 10 shop hamaco
                var check = db.VW_DeXuatNhapHang.Where(n => n.DDH_time >= dt_bd && n.DDH_time <= dt_kt && n.Shop_npp == user.Us_name.ToLower()).ToList();
                // có 1 đứa sa054
                var checku = check.Select(n => n.Us_id).Distinct().ToList();
                // co 1 shop 970134
                var checks = check.Select(n => n.Shop_id).Distinct().ToList();
                var us = db.tbl_user.Where(n => checku.Contains(n.Us_id)).ToList();
                var shop = db.tbl_shop.Where(n => checks.Contains(n.Shop_id)).ToList();
                var dxn = db.tbl_Dondathang.Where(n => n.DDH_time >= dt_bd && n.DDH_time <= dt_kt ).ToList();
                var sku = db.tbl_sku.ToList();
                ViewBag.user = us;
                ViewBag.shop = shop;
                ViewBag.sku = sku;
                ViewBag.ddh = dxn;
                return View(check);
            }
            else
            {
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
            
        }

        public ActionResult ViewReportCanhTranhDonGian()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var user = Session["user"] as tbl_user;
            var kq = db.VW_ReportDTCTSimple.Where(n => n.DTCTIMAGE_Date >= dt_bd && n.DTCTIMAGE_Date <= dt_kt && n.Shop_npp == user.Us_name.ToLower()).ToList();            
            return View(kq);
        }
        [HttpPost]
        public ActionResult ViewReportCanhTranhDonGian(FormCollection fr)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
            DateTime dt_bd = Convert.ToDateTime(fr.Get("txt_bd") + " 00:00:00");
            DateTime dt_kt = Convert.ToDateTime(fr.Get("txt_kt") + " 23:59:59");
            ViewBag.bd = fr.Get("txt_bd");
            ViewBag.kt = fr.Get("txt_kt");
            var user = Session["user"] as tbl_user;
            var kq = db.VW_ReportDTCTSimple.Where(n => n.DTCTIMAGE_Date >= dt_bd && n.DTCTIMAGE_Date <= dt_kt && n.Shop_npp == user.Us_name.ToLower()).ToList();
            return View(kq);
        }

        public ActionResult ViewCheckReportDaily()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
            DateTime dt_bd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00");
            DateTime dt_kt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + "23:59:59");
            ViewBag.bd = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.kt = DateTime.Now.ToString("yyyy-MM-dd");
            var user = Session["user"] as tbl_user;
            if (user.Us_Role.ToLower() == "sup")
            {
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
            return View();

        }
        [HttpPost]
        public ActionResult ViewCheckReportDaily(FormCollection fr)
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
            if (user.Us_Role.ToLower() == "user")
            {
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
            return View();
        }
       

    }
}