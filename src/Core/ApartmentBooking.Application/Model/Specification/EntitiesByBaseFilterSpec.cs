﻿using ApartmentBooking.Application.Model.Specification.Filters;
using Ardalis.Specification;

namespace ApartmentBooking.Application.Model.Specification
{
    public class EntitiesByBaseFilterSpec<T, TResult> : Specification<T, TResult>
    {
        public EntitiesByBaseFilterSpec(BaseFilter filter) =>
        Query.SearchBy(filter);
    }

    public class EntitiesByBaseFilterSpec<T> : Specification<T>
    {
        public EntitiesByBaseFilterSpec(BaseFilter filter) =>
            Query.SearchBy(filter);
    }
}
