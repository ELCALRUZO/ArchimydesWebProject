using ArchimydesWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.Repositories.SysSetup.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {

        Task UpdateUser(SysUsers entity);
        Task UpdateUserAsync(SysUsers entity);

        Task<SysUsers> GetUserByEmail(string email);
        Task<SysUsers> GetUserByID(int ID);

        Task<List<SysUsers>> GetAllActiveUsers();

    }
}
