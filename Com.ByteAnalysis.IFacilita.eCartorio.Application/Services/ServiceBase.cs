using AutoMapper;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Services
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity>
    {
        protected readonly IRepositoryBase<TEntity> _repository;


        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity> CreateAsync(TEntity input)
        {
            return await _repository.CreateAsync(input);
        }

        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<TEntity> GetAsync(string id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task RemoveAsync(TEntity input)
        {
            await _repository.RemoveAsync(input);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }

        public async Task UpdateAsync(string id, TEntity input)
        {
            await _repository.UpdateAsync(id, input);
        }
    }
}
