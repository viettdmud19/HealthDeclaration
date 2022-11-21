using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Net.Mail;
using DACSN.Models;
//using OpenXmlPowerTools;
using System.Data.Entity;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace DACSN.Areas.Admin.Controllers
{
    public class NhanVienController : BaseController
    {
        // GET: Admin/NhanVien
        dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext();
        // GET: Admin/NhanVien

        //[Authorize]
        public ActionResult Index()
        {
            if (Session["NhanVien"] == null)
            {
                SetAlert("Đăng Nhập không Thành Công", "warning");
                return RedirectToAction("FormLuaChon", "LoginDN");
            }
            return View();
        }

        #region Dang Nhap
        [HttpGet]
        public ActionResult DangNhapNV()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DangNhapNV(/*NhanVienLogin nvlogin*/ FormCollection collection)
        {

            var sTenDN = collection["Email"];
            var sMatKhau = collection["Password"];

            NhanVien nv = db.NhanViens.SingleOrDefault(n => n.Email == sTenDN && n.MatKhauNV == sMatKhau);

            if (nv != null)
            {
                Session["NhanVien"] = nv;
                SetAlert("Đăng Nhập Thành Công", "success");
                return Redirect("~/Admin/NhanVien/Index");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                SetAlert("Đăng Nhập không Thành Công", "warning");
                return Redirect("/Admin/NhanVien/DangNhapNV");
            }

        }
        #endregion

        #region Dang Ky
        [HttpGet]
        public ActionResult DangKyNV()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKyNV([Bind(Exclude = "IsEmailVerified,ActivationCode")] NhanVien nv, FormCollection col)
        {
            bool Status = false;
            string message = "";

            //Model Validation
            if (ModelState.IsValid)
            {
                #region //Email is already Exist
                var isExist = IsEmailExist(nv.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exxist");
                    return View(nv);
                }
                #endregion

                #region //Generate Activation Code
                nv.ActivationCode = Guid.NewGuid();
                #endregion

                #region //Password Hashing
                //var Email = col["Email"];
                var MatKhauMV = col["MatKhauNV"];

                nv.MatKhauNV = /*Crypto.Hash(nv.MatKhauNV);*/ MatKhauMV;
                //nv.ConfirmPassword =Crypto.Hash(nv.ConfirmPassword);
                #endregion

                nv.IsEmailVerified = false;

                #region //Save data to Database
                using (dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext())
                {
                    db.NhanViens.InsertOnSubmit(nv);
                    db.SubmitChanges();

                    //return Redirect("~/Admin/NhanVien/DangNhapNV");

                    //Send Email to User
                    SendVerificationLinkEmail(nv.Email, nv.ActivationCode.ToString());
                    message = "Đăng ký được thực hiện thành công. Liên kết kích hoạt tài khoản" +
                         " đã được gửi đến id email của bạn: " + nv.Email;
                    Status = true;
                }
                #endregion
            }
            else
            {
                SetAlert("Đăng Ký không Thành Công", "warning");
                message = "Yêu cầu không hợp lệ !!!";
                //return Redirect("~/Admin/NhanVien/DangKyNV");
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(nv);

            //return Redirect("~/Admin/NhanVien/DangKyNV");
            //SetAlert("Đăng Ký Thành Công", "success");
            //return Redirect("/Admin/NhanVien/DangNhapNV");
        }


        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext())
            {
                //db.Configuration.ValidateOnSaveEnabled = false;

                var v = db.NhanViens.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SubmitChanges();

                    Status = true;
                }
                else
                {
                    ViewBag.Message = "yêu cầu không hợp lệ";
                }
            }
            ViewBag.Status = Status;

            return View();
        }



        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext())
            {
                var v = db.NhanViens.Where(a => a.Email == emailID).FirstOrDefault();
                return v != null;
            }
        }

        //Chức năng gửi liên kết xác minh (Email)
        //[NonAction]
        //public void SendVerificationLinkEmail(string emailID, string activationCode)
        //{
        //    var verifyUrl = "/Admin/NhanVien/VerifyAccount/" + activationCode;
        //    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

        //    var fromEmail = new MailAddress("nguyennhatminh26122001@gmail.com", "Dotnet Awesome"); //dotnetawesome@gmail.com
        //    var toEmail = new MailAddress(emailID);
        //    var fromEmailPassword = "01218244566";//Relace with actual password

        //    string subject = "Tài khoản của bạn đã được tạo thành công!";

        //    string body = "<br/><br/>Chúng tôi sẵn sàng cho bạn biết rằng tài khoản Dotnet Awesome của bạn là " +
        //         " thành công trong việc tạo ra. Vui lòng nhấp vào liên kết dưới đây để xác minh tài khoản của bạn" +
        //         "<br/><br/><a href='" + link + "'>" + link + "</a>";

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword),
        //    };


        //    using (var message = new MailMessage(fromEmail, toEmail)
        //    {
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = true

        //    })

        //        smtp.Send(message);
        //}

        #endregion

        #region // Forgot Password


        //Chức năng gửi liên kết xác minh (Doi MK)
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string acctivationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Admin/NhanVien/" + emailFor + "/" + acctivationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("hoangthai15956@gmail.com", "Đặng Hải Việt");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "0947848841v"; //Relace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your Account Is Successfully Created"; // Tài khoản đã được tạo thành công
                body = "<br/><br/> We are excited to tell you that your Dotnet Awesome account is" +
                    "successfully created. Please click on the below link to verify your account" +
                    "<br/><br/><a href = '" + link + "'>" + link + "</a>";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "ResetPassword";
                body = "Hi, <br/><br/> We got request for reset your account password. Please click on the below link to reset your password" +
                       "<br/><br/><a href= " + link + ">Reset Password Link</a>";
            }

            var sntp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var messge = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                sntp.Send(messge);
        }


        public ActionResult QuenMK()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QuenMK(string EmailID)
        {
            //Xac minh Email
            //tạo một liên kết
            //gửi đến Email
            string message = "";
            bool status = false;

            using (dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext())
            {
                var account = db.NhanViens.Where(a => a.Email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //gui email reset password
                    string ResetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, ResetCode, "ResetPassword");
                    account.CodeResetPassword = ResetCode;

                    // this line I have added here to avoid contirm password not match issue, as we had added a confirm password property 
                    // in cur model class in part 1

                    //db.Configuration.ValidateOnSaveEnabled = false;
                    db.SubmitChanges();
                    message = "Liên kết gửi mật khẩu đã được gửi đến bạn.";
                }
                else
                {
                    message = "Không tìm thấy tài khoản";
                }
            }
            ViewBag.Message = message;
            return View();
        }


        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page

            using (dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext())
            {
                var nv = db.NhanViens.Where(a => a.CodeResetPassword == id).FirstOrDefault();
                if (nv != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                    //return Redirect("~/Admin/NhanVien/ResetPassword");
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model, FormCollection col)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (dbDuLieuYTeBDDataContext db = new dbDuLieuYTeBDDataContext())
                {
                    var nv = db.NhanViens.Where(a => a.CodeResetPassword == model.ResetCode).FirstOrDefault();
                    if (nv != null)
                    {
                        var MatKhauMV = col["MatKhauNV"];
                        nv.MatKhauNV = model.NewPassword;
                        nv.CodeResetPassword = "";
                        //db.Configuration.ValidateOnSaveEnabled = false;
                        db.SubmitChanges();
                        SetAlert("Thay đổi mật khẩu thành công", "success");
                        return RedirectToAction("DangNhapNV", "NhanVien");
                    }
                }
            }
            else
            {
                SetAlert("Thay đổi mật khẩu không thành công", "warning");
            }
            ViewBag.Message = message;
            return View(model);
        }

        #endregion

        #region Dang xuat
        public ActionResult DangXuat()
        {
            Session.Clear();
            SetAlert("Đăng xuất Thành Công", "success");
            return RedirectToAction("FormLuaChon", "LoginDN");
        }

        // cách khác
        //[Authorize]
        //[HttpPost]
        //public ActionResult DangXuat()
        //{
        //    FormsAuthentication.SignOut();
        //    return RedirectToAction("FormLuaChon", "LoginDN");
        //}
        #endregion

        #region Doi mật khau

        //[HttpGet]
        //public ActionResult DOiMK()
        //{
        //    if (Session["NhanVien"] == null)
        //    {
        //        return RedirectToAction("FormLuaChon", "LoginDN");
        //    }
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult DOiMK( /*string CurrentPassword, string NewPassword, string ConfirmNewPassword,*/ ChangePassword changePassword/*, NhanVien nv*/, FormCollection formCollection)
        //{
        //    string CurrentPassword = formCollection["CurrentPassword"];
        //    string NewPassword = formCollection["NewPassword"];
        //    string ConfirmNewPassword = formCollection["ConfirmNewPassword"];

        //    //var del = db.NhanViens.Where(n => n.IdNV == nv.IdNV).FirstOrDefault();

        //    NhanVien nv = new NhanVien();

        //    if (nv.MatKhauNV == formCollection["CurrentPassword"])
        //    {
        //        if (NewPassword == ConfirmNewPassword)
        //        {
        //            nv.MatKhauNV = formCollection["NewPassword"];

        //            db.SubmitChanges();
        //            SetAlert("Đổi Mật Khẩu Thành Công", "success");
        //            return Redirect("/Admin/NhanVien/Index");
        //        }
        //        else
        //        {
        //            SetAlert("Mật Khẩu Mới Không Trùng Nhau", "warning");
        //            return Redirect("/Admin/NhanVien/DOiMK");
        //        }
        //    }
        //    else
        //    {
        //        SetAlert("Mật Khẩu Hiện Tại Không Trùng Nhau", "warning");
        //        return Redirect("/Admin/NhanVien/DOiMK");
        //    }
        //}
        #endregion

        public ActionResult TTCNPartial()
        {
            return PartialView();
        }

        public ActionResult TTCN()
        {
            return PartialView("TTCNPartial");
        }

        #region Thông Tin Cá Nhân

        [HttpGet]
        public ActionResult ThongTinCaNhan(int? id)
        {
            var nv = db.NhanViens.SingleOrDefault(n => n.IdNV == id);
            if (Session["NhanVien"] == null)
            {
                SetAlert("Đăng Nhập không Thành Công", "warning");
                return RedirectToAction("FormLuaChon", "LoginDN");
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThongTinCaNhan(FormCollection f)
        {
            var nv = db.NhanViens.SingleOrDefault(n => n.IdNV == int.Parse(f["iMa"]));
            var sNgayDat = DateTime.Now;

            nv.IdNV = int.Parse(f["iMa"]);
            nv.Email = f["Email"];
            nv.HovaTen = f["HovaTen"];
            nv.DiaChi = f["sDiaChi"].Replace("<p>", "").Replace("</p>", "");
            nv.SDT = f["sDienThoai"];
            nv.NgayDangKy = sNgayDat;
            db.SubmitChanges();
            //Về lại trang Quản lý sách
            SetAlert("Sửa Thành Công", "success");
            return Redirect("/Admin/NhanVien/ThongTinCaNhan");
        }

        #endregion
    }
}
