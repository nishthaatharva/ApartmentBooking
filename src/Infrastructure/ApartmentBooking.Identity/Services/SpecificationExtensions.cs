﻿using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBooking.Identity.Services
{
    public static class SpecificationExtensions
    {
        public async static Task<List<T>> ApplySpecification<T>(this IQueryable<T> queryable, ISpecification<T> specification)
        where T : class
        {
            var specificationEvaluator = new SpecificationEvaluator();
            var result = specificationEvaluator.GetQuery(queryable, specification);
            return await result.ToListAsync<T>();
        }

        public async static Task<int> ApplySpecificationCount<T>(this IEnumerable<T> source, ISpecification<T> specification)
            where T : class
        {
            var queryable = source.AsQueryable();
            var specificationEvaluator = new SpecificationEvaluator(new IEvaluator[] { WhereEvaluator.Instance });
            var result = specificationEvaluator.GetQuery(queryable, specification);
            return await result.CountAsync();
        }
    }
}
