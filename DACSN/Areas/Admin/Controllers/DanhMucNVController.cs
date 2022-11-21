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
    public class DanhMucNVController : Controller
    {
        // GET: Admin/DanhMucNV
        dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext();
        // GET: Admin/DanhMucNV
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 5;
            return View(db.DanhMucs.ToList().OrderBy(n => n.IdDM).ToPagedList(iPageNum, iPageSize));
        }

        public DanhMuc GetDM(int id)
        {
            return db.DanhMucs.Where(dm => dm.IdDM == id).SingleOrDefault();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            DanhMuc dm = db.DanhMucs.Where(n => n.IdDM == id).SingleOrDefault();
            return View(dm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                var dm = GetDM(int.Parse(Request.Form["IdDM"]));
                dm.TenDM = Request.Form["TenDM"];
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                DanhMuc dm = new DanhMuc();

                dm.TenDM = f["TenDM"];
                db.DanhMucs.InsertOnSubmit(dm);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Delete");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var dm = db.DanhMucs.SingleOrDefault(n => n.IdDM == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dm);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection f)
        {
            var dm = db.DanhMucs.SingleOrDefault(n => n.IdDM == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DanhMucs.DeleteOnSubmit(dm);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}