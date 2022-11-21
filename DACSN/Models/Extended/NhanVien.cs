using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace DACSN.Models
{
    [MetadataType(typeof(NhanVienMetadata))]
    public partial class NhanVien
    {
        public string ConfirmPassword
        {
            get; set;
        }
        //public bool IsEmailVerified
        //{
        //    get; set;
        //}
        //public System.Guid ActivationCode
        //{
        //    get; set;
        //}


    }

    public class NhanVienMetadata
    {
        //Email
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID không được để trống")]
        [DataType(DataType.EmailAddress)]
        public string Email
        {
            get; set;
        }

        //Password
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password không được để trống")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Giới hạn nhỏ nhất cho Password là 6 ký tự")]
        public string MatKhauNV
        {
            get; set;
        }

        //Confirm Password
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Giới hạn nhỏ nhất cho Password là 6 ký tự")]
        [Compare("MatKhauNV", ErrorMessage = "Xác nhận mật khẩu và mật khẩu không khớp")]
        public string ConfirmPassword
        {
            get; set;
        }

        //Họ và tên
        [Display(Name = "Ho va Ten")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bạn chưa nhập Họ và Tên")]
        public string HovaTen
        {
            get; set;
        }

        //Địa chỉ
        [Display(Name = "Dia Chi")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Địa chỉ của bạn không được để trống")]
        public string DiaChi
        {
            get; set;
        }

        // Số điện thoại
        [Display(Name = "So dien thoai")]
        [DataType(DataType.PhoneNumber)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại của bạn không được để trống")]
        public string SDT
        {
            get; set;
        }

        //Ngay dang ky
        [Display(Name = "Ngay dang ky")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ngày Đăng ký không được để trống")]
        public string NgayDangKy
        {
            get; set;
        }
    }
}