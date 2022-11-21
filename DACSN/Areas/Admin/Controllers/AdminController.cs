using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACSN.Models;

namespace DACSN.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            return View();
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var sTenDN = collection["UserName"];
            var sMatKhau = collection["Password"];

            TaiKhoan ad = db.TaiKhoans.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);
            if (ad != null && ad.Quyen == "Admin")
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Admin");
            }
            else if (ad != null && ad.Quyen == "User")
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "User");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session.Clear();

            return RedirectToAction("DangNhap");
        }
        
        [HttpGet]
        public ActionResult QuenMK()
        {
            return View();
        }
       
    }
}