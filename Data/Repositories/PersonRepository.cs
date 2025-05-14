using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Entity.Contexts;
using Entity.DTOs.Person;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(ApplicationDbContext context, ILogger<Repository<Person>> logger)
         : base(context, logger)
        {
            
        }

        public async Task<bool> PatchPersonAsync(PersonUpdateDto dto)
        {
            try
            {
                var person = await _dbSet.FirstOrDefaultAsync(p => p.Id == dto.Id && p.DeleteDate == null);
                if (person == null) return false;

                // Actualizar los campos
                person.FirstName = dto.FirstName;
                person.SecondName = dto.SecondName;
                person.FirstLastName = dto.FirstLastName;
                person.SecondLastName = dto.SecondLastName;
                person.PhoneNumber = dto.PhoneNumber;
                person.NumberIdentification = dto.NumberIdentification;

                _dbSet.Update(person);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar parcialmente la persona");
                return false;
            }
        }


        public async Task<Person> GetByDocumentAsync(long numberIdentification)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.NumberIdentification == numberIdentification && p.DeleteDate == null);
        }
    }

}
