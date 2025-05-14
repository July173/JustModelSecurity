using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories   
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly ILogger<Repository<T>> _logger;

        public Repository(ApplicationDbContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        public virtual async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public virtual async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public virtual async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


        public virtual async Task<bool> SetActiveAsync(int id, bool active)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity is not IActivable activableEntity)
                    return false;

                activableEntity.Active = active;
                _context.Entry(activableEntity).Property(p => p.Active).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado activo de la entidad {EntityType}", typeof(T).Name);
                return false;
            }
        }
    }

}
