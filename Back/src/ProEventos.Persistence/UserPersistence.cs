using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Repository;

namespace ProEventos.Persistence
{
    public class UserPersistence : GeralPersitence, IUserPersistence
    {
        private readonly ProEventosContext _context;

        public UserPersistence(ProEventosContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == userName.ToLower());
        }
   
    }
}