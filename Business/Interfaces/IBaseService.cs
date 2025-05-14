using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Interfaces;

namespace Business.Interfaces
{
    public interface IBaseService<TDto, TEntity> 
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(int id);
        Task AddAsync(TDto dto);
        Task UpdateAsync(TDto dto);
        Task DeleteAsync(int id);
        Task<bool> SetActiveAsync<TActiveDto>(TActiveDto dto) where TActiveDto : IActiveDto;

    }

}
