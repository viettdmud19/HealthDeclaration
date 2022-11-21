using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DACSN.Models
{
    public class CountryClass
    {
        public int IdProvince { get; set; }

        public int IdDistrict { get; set; }

        public int IdWard { get; set; }

        //[Display(Name = "Họ Và Tên")]
        //[Required(ErrorMessage = " (*).Xin nhập Họ và tên .")]
        //public string HovaTen { get; set; }

        //[Display(Name = "Số hộ chiếu / CMND / CCCD")]
        //[Required(ErrorMessage = "(*).Cung cấp Số hộ chiếu / CMND / CCCD .")]
        //public string CMND { get; set; }

        //[Display(Name = "Năm sinh")]
        //[Required(ErrorMessage = " (*).Ngày sinh chưa nhập.")]
        //public string NamSinh { get; set; }

        //[Display(Name = "Giới Tính")]
        //[Required(ErrorMessage = " (*).Please nhập giới tính.")]
        //public string GioiTinh { get; set; }

        //[Display(Name = "Quốc Tịch")]
        //public string QuocTich { get; set; }

        //[Display(Name = "Địa Chỉ")]
        //[Compare("PhuongXa", ErrorMessage = "(*).Hãy chọn Phường Xã .")]
        //public string DiaChi { get; set; }

        //[Compare("QuanHuyen", ErrorMessage = "(*).Hãy chọn Quận Huyện .")]
        //[Display(Name = "Phường Xã")]
        //public string PhuongXa { get; set; }

        //[Compare("TinhThanh", ErrorMessage = "(*).Hãy chọn Tỉnh Thành .")]
        //[Display(Name = "Quận Huyện")]
        //public string QuanHuyen { get; set; }

        ////[Display(Name = "Tỉnh Thành")]
        ////[Required(ErrorMessage = "(*).Vui lòng chọn Tỉnh Thành.")]
        //public string TinhThanh { get; set; }

        //[Display(Name = "Điện Thoại")]
        //[Required(ErrorMessage = "(*).Xin nhập số điện thoại .")]
        //public string DienThoai { get; set; }

        //[Display(Name = "Email")]
        //[Required(ErrorMessage = "(*).Xin nhập địa chỉ Email .")]
        //public string Email { get; set; }

        //[Display(Name = "Tình Trạng")]
        //public string TinhTrang { get; set; }

        //public int T1 { get; set; }

        //public int T2 { get; set; }

        //public int T3 { get; set; }
    }
}