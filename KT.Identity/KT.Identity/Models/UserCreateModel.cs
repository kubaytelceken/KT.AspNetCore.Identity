﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KT.Identity.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Gereklidir")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Lütfen bir email formatı giriniz")]
        [Required(ErrorMessage = "Email Adresi Gereklidir")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola Alanı Gereklidir")]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage = "Parolalar Eşleşmiyor")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Cinsiyet Alanı Gereklidir")]
        public string Gender { get; set; }
    }
}
