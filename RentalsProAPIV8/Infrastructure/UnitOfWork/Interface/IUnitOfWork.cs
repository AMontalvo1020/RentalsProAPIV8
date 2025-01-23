using Microsoft.EntityFrameworkCore.Storage;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;

namespace RentalsProAPIV8.Infrastructure.UnitOfWork.Interface
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ICompanyRepository CompanyRepo { get; }
        //IExpensesRepository ExpenseRepo { get; }
        IPropertyRepository PropertyRepo { get; }
        IPropertyTypeRepository PropertyTypeRepo { get; }
        IPropertyStatusRepository PropertyStatusRepo { get; }
        IPaymentStatusRepository PaymentStatusRepo { get; }
        ILeaseRepository LeaseRepo { get; }
        //IImageRepository ImageRepo { get; }
        IUserRepository UserRepo { get; }
        IUnitRepository UnitRepo { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        Task RollbackTransactionAsync(IDbContextTransaction transaction);
        Task SaveChangesAsync();
    }
}
