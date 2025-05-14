using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Interfaces;

namespace Entity.Model
{
    public class User : IActivable, IActiveDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public ICollection<UserRol> UserRol { get; set; }

    }
}