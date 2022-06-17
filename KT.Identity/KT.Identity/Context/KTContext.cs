using KT.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KT.Identity.Context
{
    public class KTContext  : IdentityDbContext<AppUser,AppRole,int>
    {
        public KTContext(DbContextOptions<KTContext> options) : base(options)
        {

        }
    }
}
