using ArchimydesWeb.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRoleRepository Role { get; }

        IUserRepository User { get; }
        ISysUserRepository CreateUser { get; }

        int Complete();



    }
}
