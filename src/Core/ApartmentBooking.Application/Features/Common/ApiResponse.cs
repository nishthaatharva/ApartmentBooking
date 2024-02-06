using ApartmentBooking.Application.Contracts.Responses;

namespace ApartmentBooking.Application.Features.Common
{
    public class ApiResponse<T> : Response, IDataResponse<T>
    {
        public T Data { get; set; }

        public List<string>? Messages { get; set; }
    }
}
