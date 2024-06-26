﻿using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T>(DatabaseContext context)
        : IRepository<T>
        where T : BaseEntity
    {           

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public Task<T> AddAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            var entry = context.Set<T>().Add(entity);
            context.SaveChanges();
            return Task.FromResult(entry.Entity);
        }

        public Task UpdateAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
