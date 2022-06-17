using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KT.Identity.Models
{
    public class UserSignInModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Gereklidir.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Parola Gereklidir.")]
        public string Password { get; set; }
    }
}
