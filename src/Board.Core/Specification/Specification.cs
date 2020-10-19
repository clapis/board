using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Board.Core.Entities;

namespace Board.Core.Specification
{
    public class Specification<T> : ISpecification<T> where T : BaseEntity
    {
        public int? Page { get; private set; }
        public int? PageSize { get; private set; }
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public List<Expression<Func<T, bool>>> Criterias { get; private set; }
        public List<Expression<Func<T, object>>> Includes { get; private set; }

        public Specification()
        {
            Criterias = new List<Expression<Func<T, bool>>>();
            Includes = new List<Expression<Func<T, object>>>();
        }

        public void AddCriteria(Expression<Func<T, bool>> criteria) => Criterias.Add(criteria);

        public void AddInclude(Expression<Func<T, object>> include) => Includes.Add(include);

        public void ApplyOrder(Expression<Func<T, object>> order) => OrderBy = order;

        public void ApplyPaginate(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }

}
