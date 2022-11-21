using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DACSN.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Bạn chưa nhập Email !!!")]
        [EmailAddress(ErrorMessage = "Email bạn vừa nhập không đúng định dạng !!!")]
        public string Email { get; set; }
    }
}