using ApartmentBooking.Application.Contracts.Responses;

namespace ApartmentBooking.Application.Features.Common
{
    public class Response : IResponse
    {
        public bool Success { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
