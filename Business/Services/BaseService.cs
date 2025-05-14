using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Data;
using Data.Interfaces;
using Entity.Model.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Business.Services
{
    public  class BaseService<TDto, TEntity> : IBaseService<TDto, TEntity> where TEntity : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }
        public async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);

            // Asignar DateTime.Now si existe la propiedad CreateDate
            var property = typeof(TEntity).GetProperty("CreateDate");
            if (property != null && property.PropertyType == typeof(DateTime))
            {
                property.SetValue(entity, DateTime.Now);
            }

            await _repository.CreateAsync(entity);
        }

        public async Task UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            _repository.DeleteAsync(id);
        }

        public virtual async Task<bool> SetActiveAsync<TActiveDto>(TActiveDto dto)
         where TActiveDto : IActiveDto
        {
            return await _repository.SetActiveAsync(dto.Id, dto.Active);
        }



    }

}
