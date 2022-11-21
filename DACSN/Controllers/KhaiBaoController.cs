using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACSN.Models;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace DACSN.Controllers
{
    public class KhaiBaoController : Controller
    {
        dbDuLieuYTeBDDataContext data = new dbDuLieuYTeBDDataContext();

        [HttpGet]
        // GET: KhaiBao
        public ActionResult KhaiBao()
        {
            List<Province> ProvinceList = data.Provinces.ToList();
            ViewBag.ProvinceList = new SelectList(ProvinceList, "Id", "Name");
            return View();

        }

        public ActionResult GetDistrictList(int IdProvince)
        {

            List<District> DistrictList = data.Districts.Where(x => x.ProvinceId == IdProvince).ToList();
            ViewBag.DistrictList = new SelectList(DistrictList, "Id", "Name");
            return PartialView("DisplayDistrict");
        }
        public ActionResult GetWardList(int IdDistrict)
        {

            List<Ward> WardList = data.Wards.Where(x => x.DistrictID == IdDistrict).ToList();
            ViewBag.WardList = new SelectList(WardList, "Id", "Name");
            return PartialView("DisplayWard");
        }
        [HttpPost]
        //[CaptchaValidationActionFilter("CaptchaCode", "KhaiBaoCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult KhaiBao(FormCollection f, ThongTin_KhaiBao kb )
        {
            var sHoten = f["HovaTen"];
            var sCMND = f["CMND"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", f["ngaysinh"]);
            var sGioiTinh = f["Gender"];
            var sQuocTich = f["quoctich"];
            var sDiaChi = f["diachi"];
            var sPhuongXa = f["IdWard"];
            var sQuanHuyen = f["IdDistrict"];
            var sTinhThanh = f["IdProvince"];
            var sDienThoai = f["dienthoai"];
            var sEmail = f["email"];
            var sTinhTrang = f["Check"] + f["tinhtrang"];
            var sTiepXucCovi = f["Radio1"];
            var sNguoiNuocNgoai = f["Radio2"];
            var hosot = f["Radio3"];

             

            if(ModelState.IsValid)
            {
                kb.HoVaTen = sHoten;
                kb.CMND = sCMND;
                kb.NamSinh = DateTime.Parse(dNgaySinh);
                kb.GioiTinh = sGioiTinh;
                kb.QuocTich = sQuocTich;
                kb.PhuongXa = int.Parse(sPhuongXa);
                kb.QuanHuyen = int.Parse(sQuanHuyen);
                kb.TinhThanh = int.Parse(sTinhThanh);
                kb.DiaChi = sDiaChi;
                kb.DienThoai = sDienThoai;
                kb.Email = sEmail;
                kb.TinhTrang = sTinhTrang;
                kb.TiepXucCOVID = sTiepXucCovi;
                kb.NguoiVungCovid = sNguoiNuocNgoai;
                kb.NguoiSotHo = hosot;
                kb.NgayKhaiBao = DateTime.Now;
                data.ThongTin_KhaiBaos.InsertOnSubmit(kb);
                data.SubmitChanges();
                string subject = "---Khai Báo Thành Công---";
                string body = "Thông tin khai báo của bạn đã xác nhận .";
                WebMail.Send(sEmail, subject, body, null, null, null, true, null, null, null, null, null, null);

                return Redirect("~/KhaiBao/SuccessKhaiBao");
            }



            return this.KhaiBao();
        }
        public ActionResult SuccessKhaiBao()
        {
            return View();
        }
    }
}