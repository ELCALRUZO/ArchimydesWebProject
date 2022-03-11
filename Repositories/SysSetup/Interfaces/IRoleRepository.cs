using ArchimydesWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.Repositories.SysSetup.Interfaces
{
    public interface IRoleRepository : IRepository<SysRole>
    {
        Task<List<SysRole>> GetAllRoles(); 
        Task<SysRole> GetRole(int id);

    }
}
