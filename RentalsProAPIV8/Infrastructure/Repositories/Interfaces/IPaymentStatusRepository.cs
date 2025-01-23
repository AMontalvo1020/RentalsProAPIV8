using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories.Interfaces
{
    public interface IPaymentStatusRepository : IRepository<PaymentStatus>
    {
        Task<List<StatusDTO>> GetStatuses();
    }
}
