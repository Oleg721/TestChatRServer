using System;
using System.Collections.Generic;
using Contracts;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DTO.Contracts;

namespace DAL.Repo
{
    public class BaseRepository<T, TId, TDto> : ICrud<TId, TDto> where T : BaseEntity<TId> 
    {
        protected DbContext _context;
        protected IMapper _mapper;
        public BaseRepository(DbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TDto>> GetAsync()
        {
            var dbResult = await _context.Set<T>().ToListAsync();
            Console.WriteLine(dbResult.Count);
            List<TDto> result = _mapper?.Map<List<TDto>>(dbResult);
            return result;
        }

        public async Task<TDto> GetAsync(TId id)
        {
            var dbResult = await _context.Set<T>().FindAsync(id);
            TDto result = _mapper.Map<TDto>(dbResult);
            return result;
        }
        public async Task<TId> CreateAsync(TDto item)
        {
            T newDbItem = _mapper.Map<T>(item);
            _context.Set<T>().Add(newDbItem);
            await this._context.SaveChangesAsync();

            return newDbItem.Id;
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            var deletedEntity = await _context.Set<T>().FindAsync(id);
            if (deletedEntity == null)
            {
                return false;
            };
            _context.Remove(deletedEntity);
            var result = await _context.SaveChangesAsync();
            return result == 1 ? true : false;
        }

        public async Task<bool> UpdateAsync(TDto item)
        {
            T updatedDbItem = _mapper.Map<T>(item);
            var dbResult = await _context.Set<T>().FindAsync(updatedDbItem.Id);
            if (dbResult != null)
            {
                _mapper.Map<TDto, T>(item, dbResult);
                _context.Update(dbResult);
                var result = await _context.SaveChangesAsync();
                if (result == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
