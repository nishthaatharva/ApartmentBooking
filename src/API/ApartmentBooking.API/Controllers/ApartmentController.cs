using ApartmentBooking.API.Controllers.Base;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Apartments.Commands;
using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Application.Features.Apartments.Queries;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Identity.Authorization;
using ApartmentBooking.Identity.Authorization.Permissions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Action = ApartmentBooking.Identity.Authorization.Action;

namespace ApartmentBooking.API.Controllers;

public class ApartmentController : BaseApiController
{
    [HttpPost("Create")]
    [MustHavePermission(Action.Create, Resource.Apartment)]
    public async Task<ApiResponse<string>> CreateApartment(CreateApartmentCommandRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("Search")]
    [MustHavePermission(Action.Search, Resource.Apartment)]
    public async Task<IPagedDataResponse<ApartmentListDto>> SearchAsync(SearchApartmentQueryRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpPut("{id}")]
    [MustHavePermission(Action.Update, Resource.Apartment)]
    public async Task<ApiResponse<string>> UpdateApartment(Guid id, UpdateApartmentCommandRequest request)
    {
        if (id != request.Id)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Data = "The provided ID in the route does not match the ID in the request body.",
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
        return await Mediator.Send(request);
    }

    [HttpDelete("{id}")]
    [MustHavePermission(Action.Delete, Resource.Apartment)]
    public async Task<ApiResponse<string>> DeleteApartment(Guid id)
    {
        return await Mediator.Send(new DeleteApartmentCommandRequest(id));
    }

    [HttpGet("{id}")]
    [MustHavePermission(Action.View, Resource.Apartment)]
    public async Task<ApiResponse<ApartmentDetailsDto>> GetApartmentDetails(Guid id)
    {
        return await Mediator.Send(new GetApartmentDetailsQueryRequest(id));
    }

    [HttpGet("list")]
    [MustHavePermission(Action.View, Resource.Apartment)]
    public async Task<ApiResponse<List<ApartmentDetailsDto>>> GetAvailableApartments()
    {
        return await Mediator.Send(new GetAvailableApartmentsQuery());
    }
}
