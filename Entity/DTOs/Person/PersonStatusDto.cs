using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Interfaces;

namespace Entity.DTOs.Person
{
    public class PersonStatusDto : IActiveDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }


    }
}

