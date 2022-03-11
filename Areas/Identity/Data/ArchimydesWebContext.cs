using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchimydesWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArchimydesWeb.Data
{
    public class ArchimydesWebContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ArchimydesWebContext(DbContextOptions<ArchimydesWebContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationRole>()
           .Property(e => e.Id)
           .ValueGeneratedOnAdd();
        }

        //DB Sets should go in here for Migration Purpose
        #region "DB SETS"
        public virtual DbSet<SysUsers> SYS_Users { get; set; }
        public virtual DbSet<SysRole> SYS_Role { get; set; }

        #endregion

    }
}
