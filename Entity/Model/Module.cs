﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Interfaces;

namespace Entity.Model
{
    public class Module: IActivable, IActiveDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public ICollection<FormModule> FormModule { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
