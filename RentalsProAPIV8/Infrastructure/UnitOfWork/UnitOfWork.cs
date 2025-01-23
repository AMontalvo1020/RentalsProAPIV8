using Microsoft.EntityFrameworkCore.Storage;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Infrastructure.UnitOfWork.Interface;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RentalsProContext _context;
        private IDbContextTransaction _currentTransaction;
        public ICompanyRepository CompanyRepo { get; }
        //public IExpensesRepository ExpenseRepo { get; }
        public IPropertyRepository PropertyRepo { get; }
        public IPropertyTypeRepository PropertyTypeRepo { get; }
        public IPropertyStatusRepository PropertyStatusRepo { get; }
        public IPaymentStatusRepository PaymentStatusRepo { get; }
        public ILeaseRepository LeaseRepo { get; }
        //public IImageRepository ImageRepo { get; }
        public IUserRepository UserRepo { get; }
        public IUnitRepository UnitRepo { get; }

        public UnitOfWork(RentalsProContext context,
                          ICompanyRepository companyRepo,
                          //IExpensesRepository expenseRepository,
                          IPropertyRepository propertyRepo,
                          IPropertyStatusRepository propertyStatusRepo,
                          IPropertyTypeRepository propertyTypeRepo,
                          IPaymentStatusRepository paymentStatusRepo,
                          ILeaseRepository leaseRepo,
                          //IImageRepository imageRepo,
                          IUserRepository userRepo,
                          IUnitRepository unitRepo)
        {
            _context = context;
            CompanyRepo = companyRepo ?? throw new ArgumentNullException(nameof(companyRepo));
            //ExpenseRepo = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
            PropertyRepo = propertyRepo;
            PropertyStatusRepo = propertyStatusRepo;
            PropertyTypeRepo = propertyTypeRepo;
            PaymentStatusRepo = paymentStatusRepo;
            LeaseRepo = leaseRepo;
            //ImageRepo = imageRepo ?? throw new ArgumentNullException(nameof(imageRepo));
            UserRepo = userRepo;
            UnitRepo = unitRepo ?? throw new ArgumentNullException(nameof(unitRepo));
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return null; // Or handle this scenario based on your specific needs
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync();
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException("Transaction passed is not the current one");

            try
            {
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            try
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
        public async ValueTask DisposeAsync() => await _context.DisposeAsync();
    }
}
