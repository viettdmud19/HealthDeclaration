using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACSN.Models;

namespace DACSN.Areas.Admin.Controllers
{
    public class LoginDNController : Controller
    {
        
        dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext();
        // GET: Admin/LoginDN
        public ActionResult FormLuaChon()
        {
            return View();
        }
    }
}