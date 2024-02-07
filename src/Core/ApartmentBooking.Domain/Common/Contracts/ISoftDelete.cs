namespace ApartmentBooking.Domain.Common.Contracts
{
    public interface ISoftDelete
    {
        string? DeletedBy { get; set; }
        DateTime DeletedOn { get; set; }
        bool IsDeleted { get; set; }
    }
}
