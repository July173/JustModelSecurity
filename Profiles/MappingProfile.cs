using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entity.Model;
using Entity.DTOs.Person;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {


            CreateMap<Person, PersonDto>();
            CreateMap<PersonDto, Person>();
            CreateMap<PersonUpdateDto, PersonDto>();
            CreateMap<Person, PersonUpdateDto>();


            // etc...
        }
    }
}