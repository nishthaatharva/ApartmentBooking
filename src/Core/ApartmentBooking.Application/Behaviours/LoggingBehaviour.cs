﻿using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ApartmentBooking.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //Request
        _logger.LogInformation($"Handling {typeof(TRequest).Name}");
        Type myType = request.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        foreach (PropertyInfo prop in props)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            object propvalues = prop.GetValue(request, null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
           // _logger.LogInformation("{property} : {@Value}", prop.Name, propvalues);
        }

        var response = await next();

        //Response
        _logger.LogInformation($"Handled {typeof(TResponse).Name}");
        return response;
    }
}
