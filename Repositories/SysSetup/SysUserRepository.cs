using ArchimydesWeb.Data;
using ArchimydesWeb.Models;
using ArchimydesWeb.Repositories.SysSetup.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.Repositories.SysSetup
{
    public class SysUserRepository : Repository<SysUsers>, ISysUserRepository
    {
        ArchimydesWebContext _context;

        public SysUserRepository(ArchimydesWebContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateUser(SysUsers entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {

            }
        }

    }
}
