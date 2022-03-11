using ArchimydesWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.Repositories.SysSetup.Interfaces
{
    public interface ISysUserRepository : IRepository<SysUsers>
    {
        void UpdateUser(SysUsers entity);

    }
}
