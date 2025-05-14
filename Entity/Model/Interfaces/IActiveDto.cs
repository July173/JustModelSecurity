using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model.Interfaces
{
    public interface IActiveDto
    {
        int Id { get; set; }
        bool Active { get; set; }
    }

}
