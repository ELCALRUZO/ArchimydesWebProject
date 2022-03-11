using ArchimydesWeb.Data;
using ArchimydesWeb.Repositories.SysSetup;
using ArchimydesWeb.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchimydesWeb.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ArchimydesWebContext _context;

        #region "Lock"
        private static readonly object _Instancelock = new object();
        #endregion

        #region "Interfaces"
         
        IRoleRepository _Role; 
        IUserRepository _User;
        ISysUserRepository _CreateUser; 


        #endregion

        public UnitOfWork(ArchimydesWebContext context)
        {
            _context = context;

        }


        public IUserRepository User
        {
            get
            {
                if (_User == null)
                {
                    lock (_Instancelock)
                    {
                        if (_User == null)
                            _User = new UserRepository(_context);
                    }
                }

                return _User;
            }
        }

        public ISysUserRepository CreateUser
        {
            get
            {
                if (_CreateUser == null)
                {
                    lock (_Instancelock)
                    {
                        if (_CreateUser == null)
                            _CreateUser = new SysUserRepository(_context);
                    }
                }

                return _CreateUser;
            }
        }



        public IRoleRepository Role
        {
            get
            {
                if (_Role == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Role == null)
                            _Role = new RoleRepository(_context);
                    }
                }

                return _Role;
            }
        }



        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }






    }
}
