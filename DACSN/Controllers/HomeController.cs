using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACSN.Models;

namespace DACSN.Controllers
{
    public class HomeController : Controller
    {
        dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext();
        // GET: Home
        
        //tintuc
       
        public ActionResult TinTucPartial()
        {
            var ListTinTuc = db.BaiViets.Where(s => s.IdDM == 2).OrderByDescending(s => s.NgayViet).Take(3);
            return View(ListTinTuc);
        }
        
        public ActionResult ChiDaoPartial()
        {
            var ListChiDao = db.BaiViets.Where(s => s.IdDM == 1).OrderByDescending(s => s.NgayViet);
            return View(ListChiDao);
        }
        
        public ActionResult Index()
        {        
            return View();
        }

        public ActionResult BanTinPartial()
        {
            var ListTinTuc = db.BaiViets.Where(s => s.IdDM == 2).OrderByDescending(s => s.NgayViet).Take(4);
            return View(ListTinTuc);
        }

        public ActionResult BanTinMoiNhatPartial()
        {
            var ListTinTuc = db.BaiViets.Where(s => s.IdDM == 2).OrderByDescending(s => s.NgayViet).Take(1);
            return View(ListTinTuc);
        }

        public ActionResult VideoMoiPartial()
        {
            var ListVideo = db.BaiViets.Where(s => s.IdDM == 3).OrderByDescending(s => s.NgayViet).Take(1);
            return View(ListVideo);
        }

        public ActionResult VideoPartial()
        {
            var ListVideo = db.BaiViets.Where(s => s.IdDM == 3).OrderBy(s => s.NgayViet).Take(3);
            return View(ListVideo);
        }

        public ActionResult VaccineMoiPartial()
        {
            var ListVideo = db.BaiViets.Where(s => s.IdDM == 4).OrderByDescending(s => s.NgayViet).Take(1);
            return View(ListVideo);
        }

        public ActionResult VaccinePartial()
        {
            var ListVideo = db.BaiViets.Where(s => s.IdDM == 4).OrderBy(s => s.NgayViet).Take(3);
            return View(ListVideo);
        }

        public ActionResult ChiTietTinTuc(int id)
        {
            var bv = from s in db.BaiViets
                       where s.IdBV == id
                       select s;
            return View(bv.Single());
        }

        public ActionResult Video()
        {
            var ListVideo = db.BaiViets.Where(s => s.IdDM == 3).OrderByDescending(s => s.NgayViet).Take(9);
            return View(ListVideo);
        }

        public ActionResult ChiDao()
        {
            var ListChiDao = db.BaiViets.Where(s => s.IdDM == 1).OrderByDescending(s => s.NgayViet).Take(9);
            return View(ListChiDao);
        }

        public ActionResult Vaccine()
        {
            var ListVideo = db.BaiViets.Where(s => s.IdDM == 4).OrderByDescending(s => s.NgayViet).Take(9);
            return View(ListVideo);
        }

        public ActionResult TinTuc()
        {
            var ListTinTuc = db.BaiViets.Where(s => s.IdDM == 2).OrderByDescending(s => s.NgayViet).Take(9);
            return View(ListTinTuc);
        }

        public ActionResult BieuDo()
        {
            return View();
        }
        public ActionResult List_ThongKe()
        {
            return View();
        }
       
        public ActionResult DienBienDich()
        {
            var ListDienBien = LayDienBienDich(3);
            return View(ListDienBien);
        }

        private List<DienBienDich> LayDienBienDich(int count)
        {
            return db.DienBienDiches.OrderByDescending(a => a.ThoiGian).Take(count).ToList();
        }
    }
}