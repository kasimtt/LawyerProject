using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; 
        public string? ProfileImage { get; set; } 
        public bool IsAdmin { get; set; } // Daha sonra belki yeni entity acarız admin diye ve user'dan kalıtırız
        public ICollection<Case>? Cases { get; set; }    
    }
}
