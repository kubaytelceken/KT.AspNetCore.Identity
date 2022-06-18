using KT.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KT.Identity.TagHelpers
{
    [HtmlTargetElement("getUserInfo")]
    public class AddUserInfo : TagHelper
    {
        public int userId { get; set; }
        private readonly UserManager<AppUser> _userManager;

        public AddUserInfo(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public  override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var html = "";
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var item in roles)
            {
                html += item + " ";
            }

            output.Content.SetHtmlContent(html);
        }
    }
}
