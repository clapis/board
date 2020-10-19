using System;
using System.Linq.Expressions;
using Board.Core.Entities;

namespace Board.Core.Specification
{
    /// <summary>
    /// Supports creation of a Specification using a fluent builder style
    /// E.g.   var spec = new SpecificationBuilder<T>()
    ///                     .Where(t => t.Property == false)
    ///                     .Include(t => t.NavigationProperty)
    ///                     .Paginate(1, 10)
    ///                     .Build();
    /// </summary>
    public class SpecificationBuilder<T> where T : BaseEntity
    {
        private Specification<T> _specification = new Specification<T>();

        public SpecificationBuilder<T> Where(Expression<Func<T, bool>> criteria)
            => Bind(spec => spec.AddCriteria(criteria));

        public SpecificationBuilder<T> Include(Expression<Func<T, object>> include)
            => Bind(spec => spec.AddInclude(include));

        public SpecificationBuilder<T> OrderBy(Expression<Func<T, object>> order)
            => Bind(spec => spec.ApplyOrder(order));

        public SpecificationBuilder<T> Paginate(int page, int pageSize)
            => Bind(spec => spec.ApplyPaginate(page, pageSize));

        public Specification<T> Build() => _specification;

        private SpecificationBuilder<T> Bind(Action<Specification<T>> action)
        {
            action(_specification);
            return this;
        }
    }
}
