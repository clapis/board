using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Board.Core.Entities;

namespace Board.Core.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        int? Page { get; }
        int? PageSize { get; }
        Expression<Func<T, object>> OrderBy { get; }
        List<Expression<Func<T, bool>>> Criterias { get; }
        List<Expression<Func<T, object>>> Includes { get; }
    }
}