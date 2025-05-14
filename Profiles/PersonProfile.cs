using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entity.DTOs.Person;
using Entity.Model;

namespace Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {

            CreateMap<Person, PersonDto>();
            CreateMap<PersonDto, Person>();
            CreateMap<Person, PersonUpdateDto>();
            CreateMap<PersonUpdateDto, Person>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
