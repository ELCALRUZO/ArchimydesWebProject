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
    public class RoleRepository : Repository<SysRole>, IRoleRepository
    {
        ArchimydesWebContext _context;

        public RoleRepository(ArchimydesWebContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SysRole>> GetAllRoles()
        {
            try
            {
                var allRole = await Context.SYS_Role
                            .AsNoTracking()
                            .ToListAsync();

                return allRole;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        public async Task<SysRole> GetRole(int id)
        {
            try
            {
                var idRole = await Context.SYS_Role
                            .Where(u => u.RoleID == id) 
                            .AsNoTracking()
                             .FirstOrDefaultAsync(); 

                return idRole;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
