namespace ApartmentBooking.Domain.Common.Contracts
{
    public interface IAuditableEntity
    {
        DateTime CreatedOn { get; set; }
        string? CreatedBy { get; set; }
        DateTime ModifiedOn { get; set; }
        string? ModifiedBy { get; set; }
    }
}
