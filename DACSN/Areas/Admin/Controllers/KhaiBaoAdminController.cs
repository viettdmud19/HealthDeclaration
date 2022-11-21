using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACSN.Models;
using System.IO;
using PagedList;
using System.Data.Entity;

namespace DACSN.Areas.Admin.Controllers
{
    public class KhaiBaoAdminController : Controller
    {
        dbDuLieuYTeBDDataContext data = new dbDuLieuYTeBDDataContext();

        // GET: Admin/KhaiBaoYTe
        public ActionResult KhaiBaoYTe(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 30;
            var kq = data.ThongTin_KhaiBaos.ToList().OrderByDescending(n => n.NgayKhaiBao);
            ViewBag.TongSoLuong = kq.Count();
            return View(kq.ToPagedList(iPageNum, iPageSize));
           

        }
        //public ActionResult SL_KhaiBao()
        //{
        //    return PartialView();
        //}
        public ActionResult SearchSl_KhaiBao(string searchstring  ,int? page, int IdDistrict=0  )
        {
           
            int iPageNum = (page ?? 1);
            int iPageSize = 30;
            var dl = data.ThongTin_KhaiBaos.Include(b => b.Province).Include(b => b.District);
            ViewBag.Search = searchstring;
            
            if (!String.IsNullOrEmpty(searchstring))
                {
                var kq = from s in data.ThongTin_KhaiBaos where s.TinhTrang.Contains(searchstring)  select s;
                ViewBag.Count = kq.Count();

                //var kq = from s in data.ThongTin_KhaiBaos where s.NgayKhaiBao.ToString().Contains(strSearch)   select s ;
                return View(kq.ToPagedList(iPageNum, iPageSize));
            }

            //else if (String.IsNullOrEmpty(searchstring) && !String.IsNullOrEmpty(strSearch))
            //{
            //    var kq = from s in data.ThongTin_KhaiBaos where s.HoVaTen.Contains(strSearch) select s;
            //    return View(kq.ToPagedList(iPageNum, iPageSize));
            //}
            //    if(IdDistrict != 0)
            //{
            //    dl = dl.Where(c => c.QuanHuyen == IdDistrict);
            //}
            //ViewBag.IdDistrict = new SelectList(data.Districts, "Id", "Name"); // danh sách Category               
            return View(dl.ToList().ToPagedList(iPageNum, iPageSize));
            
            
           
        }
      
        public ActionResult ThongKe()
        {
            
           
            return View();
        }

        public ActionResult ThongKe_BinhDuong()
        {
            ViewBag.TDM   = (from s in data.ThongTin_KhaiBaos where s.QuanHuyen==718   select s).Count();
            ViewBag.DT = (from s in data.ThongTin_KhaiBaos where s.QuanHuyen == 720 select s).Count();
            ViewBag.BC = (from s in data.ThongTin_KhaiBaos where s.QuanHuyen == 721 select s).Count();
            ViewBag.PG = (from s in data.ThongTin_KhaiBaos where s.QuanHuyen == 722 select s).Count();
            ViewBag.TU = (from s in data.ThongTin_KhaiBaos where s.QuanHuyen == 723 select s).Count();
            ViewBag.DA = (from s in data.ThongTin_KhaiBaos where s.QuanHuyen == 724 select s).Count();
            ViewBag.TA = (from s in data.ThongTin_KhaiBaos where s.QuanHuyen == 725 select s).Count();
            //74
            return PartialView();
        }
        public ActionResult ThongKe_BinhDuong_2()
        {
            ViewBag.Tiepxuc = (from s in data.ThongTin_KhaiBaos where s.TiepXucCOVID.ToString().Contains("true") select s).Count();
            ViewBag.Vungdich = (from s in data.ThongTin_KhaiBaos where s.NguoiVungCovid.ToString().Contains("true") select s).Count();
            ViewBag.Trieuchung = (from s in data.ThongTin_KhaiBaos where s.NguoiSotHo.ToString().Contains("true") select s).Count();
            ViewBag.TinhTrang = (from s in data.ThongTin_KhaiBaos where s.TinhTrang.Contains("dấu hiệu bất thường") select s).Count();
            //var taisan = from s in data.ThongTin_KhaiBaos
            //             from x in data.Districts
            //             //from y in _db.DONVITAISANs
            //             //from z in _db.DONVIs
            //             where s.QuanHuyen== x.Id
            //             //where y.MaTS == s.MaTS
            //             //where z.MaDV == y.MaDV
            //             select new
            //             {
            //                 //TenTS = s.TenTS,
            //                 SoLuong = s.d.FirstOrDefault().Soluong,
            //                 //TenDV = (s.DONVITAISANs.FirstOrDefault()).DONVI.TenDV,
            //                 //SoLuongDV = s.DONVITAISANs.FirstOrDefault().Soluong,
            //                 //ConLai = (s.GIATRITAISANs.FirstOrDefault().Soluong - s.DONVITAISANs.FirstOrDefault().Soluong)

            //             };

            return PartialView();
        }

        public ActionResult thongke_TiepXuc(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 30;
            var Tiepxuc = from s in data.ThongTin_KhaiBaos where s.TiepXucCOVID.ToString().Contains("true") select s;
            return PartialView(Tiepxuc.ToList().ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult thongke_Vungdich(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 30;
            var Tiepxuc = from s in data.ThongTin_KhaiBaos where s.NguoiVungCovid.ToString().Contains("true") select s;
            return PartialView(Tiepxuc.ToList().ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult thongke_Trieuchung(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 30;
            var Tiepxuc = from s in data.ThongTin_KhaiBaos where s.NguoiSotHo.ToString().Contains("true") select s;
            return PartialView(Tiepxuc.ToList().ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult DanhSach_NV(int ? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 30;
            var kq = data.NhanViens.ToList().OrderByDescending(n => n.NgayDangKy);
            ViewBag.TongTV = kq.Count();
            return View(kq.ToPagedList(iPageNum, iPageSize));
        }

    }
}