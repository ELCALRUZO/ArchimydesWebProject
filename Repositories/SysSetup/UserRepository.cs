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
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        ArchimydesWebContext _context;

        public UserRepository(ArchimydesWebContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateUserAsync(SysUsers entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();


        }
        public async Task UpdateUser(SysUsers entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }


        public async Task<SysUsers> GetUserByEmail(string email)
        {
            try
            {
                var user = await Context.SYS_Users 
                                .Where(u => u.Email == email)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<SysUsers> GetUserByID(int ID)
        {
            try
            {
                var user = await Context.SYS_Users 
                                .Where(u => u.UserID == ID)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<SysUsers>> GetAllActiveUsers()
        {
            try
            {
                var users = await Context.SYS_Users 
                                .Where(u => !string.IsNullOrEmpty(u.Email))
                                .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }







    }
}
