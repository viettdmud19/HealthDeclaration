using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACSN.Models;
using System.IO;
using PagedList;

namespace DACSN.Areas.Admin.Controllers
{
    public class BaiVietController : Controller
    {
        dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext();
        // GET: Admin/BaiViet
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 5;
            var ketqua = db.BaiViets.ToList().OrderBy(n => n.IdBV);
            ViewBag.Ketqua = ketqua.Count();
            return View(ketqua.ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IdDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "IdDM", "TenDM");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(BaiViet bv, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            //đưa dữ liệu vào DropDown
            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "IdDM", "TenDM");

            if (fFileUpload == null)
            {
                // nội dung thông báo yêu cầu chọn ảnh bìa
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                //lưu thông tin để khi load lại trang do yêu cầu
                // chọn ảnh bìa sẽ hiển thị các thông tin này lên trang
                ViewBag.TieuDe = f["sTieuDe"];
                ViewBag.TomTat = f["sTomTat"];
                ViewBag.NoiDung = f["sNoiDung"];
                ViewBag.LinkVideo = f["sLinkVideo"];
                ViewBag.NguoiViet = f["sNguoiViet"];
                ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "IdDM", "TenDM", f["MaDM"]);
                ViewBag.Tag = f["sTag"];
                return View();

            }
            else
            {
                if (ModelState.IsValid)
                {
                    // lấy tên file (khai báo thư viện :System.Io
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    // lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    // kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    // lưu Sách vào csdl
                    bv.TieuDe = f["sTieuDe"];
                    bv.TomTat = f["sTomTat"];
                    bv.NoiDung = f["sNoiDung"];
                    bv.Hinh = sFileName;
                    bv.NguoiViet = f["sNguoiViet"];
                    bv.NgayViet = DateTime.Now;
                    bv.Tag = f["sTag"];
                    bv.LinkVideo = f["sLinkVideo"];
                    bv.NguoiViet = f["sNguoiViet"];
                    bv.IdDM = int.Parse(f["IdDM"]);
                    db.BaiViets.InsertOnSubmit(bv);
                    db.SubmitChanges();
                    // về lại trang quản lý sản phẩm
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            var bv = db.BaiViets.SingleOrDefault(n => n.IdBV == id);
            if (bv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bv);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var bv = db.BaiViets.SingleOrDefault(n => n.IdBV == id);
            if (bv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bv);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int? id, FormCollection f)
        {
            var bv = db.BaiViets.SingleOrDefault(n => n.IdBV == id);

            if (bv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            db.BaiViets.DeleteOnSubmit(bv);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var bv = db.BaiViets.SingleOrDefault(n => n.IdBV == id);
            if (bv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.IdDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "IdDM", "TenDM");
            return View(bv);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var bv = db.BaiViets.SingleOrDefault(n => n.IdBV == int.Parse(f["iIdBV"]));
            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "IdDM", "TenDM", f["MaDM"]);
            if (ModelState.IsValid)
            {
                if (fFileUpload != null) //Kiểm tra để xác nhận cho thay đổi ảnh bìa
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    //Kiểm tra file đã tồn tại chưa
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    bv.Hinh = sFileName;
                }
                //Lưu Sach vào CSDL
                bv.TieuDe = f["sTieuDe"];
                bv.TomTat = f["sTomTat"];
                bv.NoiDung = f["sNoiDung"];
                bv.NgayViet = DateTime.Now;
                bv.NguoiViet = f["sNguoiViet"];
                bv.LinkVideo = f["sLinkVideo"];
                bv.Tag = f["sTag"];
                bv.IdDM = int.Parse(f["IdDM"]);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(bv);
        }
        public ActionResult Search_BaiViet(string strSearch, int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 30;
            ViewBag.search_BV = strSearch;
            var dl = db.BaiViets;
            if (!String.IsNullOrEmpty(strSearch))
            {
                var kq = from s in db.BaiViets where s.TieuDe.Contains(strSearch)|| s.NguoiViet.Contains(strSearch) || s.Tag.Contains(strSearch) select s;
                ViewBag.Count_KQ = kq.Count();

                //var kq = from s in data.ThongTin_KhaiBaos where s.NgayKhaiBao.ToString().Contains(strSearch)   select s ;
                return View(kq.ToPagedList(iPageNum, iPageSize));
            }
            return View(dl.ToList().ToPagedList(iPageNum, iPageSize));
        }
    }
}