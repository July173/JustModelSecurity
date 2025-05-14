using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Data;
using Data.Interfaces;
using Data.Repositories;
using Entity.DTOs.Person;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services
{
    public class PersonService : BaseService<PersonUpdateDto, Person>, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IPersonRepository personRepository, IMapper mapper, ILogger<PersonService> logger)
            : base(personRepository, mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> PatchPersonAsync(PersonUpdateDto dto)
        {
            return await _personRepository.PatchPersonAsync(dto);
        }

        public async Task AddFromCreateDtoAsync(PersonDto dto)
        {
            var existing = await _personRepository.GetByDocumentAsync(dto.NumberIdentification);
            if (existing != null)
                throw new ValidationException("El número de documento ya está registrado.");

            var entity = _mapper.Map<Person>(dto);

            var property = typeof(Person).GetProperty("CreateDate");
            if (property != null && property.PropertyType == typeof(DateTime))
            {
                property.SetValue(entity, DateTime.Now);
            }

            await _personRepository.CreateAsync(entity);



        }
    }
}

