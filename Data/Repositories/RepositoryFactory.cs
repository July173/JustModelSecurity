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

namespace Data.Repositories
{
    public class RepositoryFactory<T> : IRepositoryFactory<T> where T: class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryFactory(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

    public async Task<bool> PatchPersonAsync(PersonUpdateDto dto)
        {
            try
            {
                var person = await _context.Set<Person>().FirstOrDefaultAsync(p => p.Id == dto.Id && p.DeleteDate == null);
                if (person == null)
                    return false;

                // Actualizar solo los campos enviados desde el DTO
                person.FirstName = dto.FirstName;
                person.SecondName = dto.SecondName;
                person.FirstLastName = dto.FirstLastName;
                person.SecondLastName = dto.SecondLastName;
                person.PhoneNumber = dto.PhoneNumber;
                person.NumberIdentification = dto.NumberIdentification;

                _dbSet.UpdatePartialAsync(person);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar parcialmente la persona");
                return false;
            }
        }

    }
}
