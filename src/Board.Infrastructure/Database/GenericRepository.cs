using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Board.Core.Abstractions;
using Board.Core.Entities;
using Board.Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Database
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly BoardDbContext _context;

        public GenericRepository(BoardDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await GetQuery(spec).CountAsync();
        }

        public async Task<T> FirstAsync(ISpecification<T> spec)
        {
            return await GetQuery(spec).FirstAsync();
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            return await GetQuery(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await GetQuery(spec).ToListAsync();
        }

        private IQueryable<T> GetQuery(ISpecification<T> spec)
        {
            var query = _context.Set<T>().AsQueryable();

            if (spec.Criterias.Any())
                query = spec.Criterias.Aggregate(query, (q, criteria) => q.Where(criteria));

            if (spec.Includes.Any())
                query = spec.Includes.Aggregate(query, (q, include) => q.Include(include));

            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.Page.HasValue && spec.PageSize.HasValue)
                query = query.Skip(spec.Page.Value * spec.PageSize.Value).Take(spec.PageSize.Value);

            return query;
        }

        
    }
}
