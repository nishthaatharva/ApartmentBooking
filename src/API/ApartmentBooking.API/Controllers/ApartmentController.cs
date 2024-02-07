using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Apartments.Commands;
using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Application.Features.Apartments.Queries;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.Model.Specification.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApartmentBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Create")]
        public async Task<ApiResponse<string>> CreateApartment(CreateApartmentCommandRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("Search")]
        public async Task<IPagedDataResponse<ApartmentListDto>> SearchAsync(PaginationFilter request)
        {
            return await _mediator.Send(new SearchApartmentQueryRequest() { PaginationFilter = request });
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<string>> UpdateApartment(Guid id, UpdateApartmentCommandRequest request)
        {
            if (id != request.apartment.Id)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Data = "The provided ID in the route does not match the ID in the request body.",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            return await _mediator.Send(request);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteApartment(Guid id)
        {
            return await _mediator.Send(new DeleteApartmentCommandRequest(id));
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<ApartmentDetailsDto>> GetApartmentDetails(Guid id)
        {
            return await _mediator.Send(new GetApartmentDetailsQueryRequest(id));
        }
    }
}
