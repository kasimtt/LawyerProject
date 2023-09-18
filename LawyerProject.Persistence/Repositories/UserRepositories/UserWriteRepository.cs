using LawyerProject.Application.Repositories.UserRepositories;
using LawyerProject.Domain.Entities;
using LawyerProject.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Persistence.Repositories.UserRepositories
{
    public class UserWriteRepository : WriteRepository<User>, IUserWriteRepository
    {
        private readonly LawyerProjectContext _context;
        public UserWriteRepository(LawyerProjectContext context) : base(context)
        {
            _context = context;
        }
    }
}
