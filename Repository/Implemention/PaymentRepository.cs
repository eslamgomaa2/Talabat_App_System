using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTransactionAsync(PaymentTransaction transaction)
        {
            await _context.PaymentTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<PaymentTransaction> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.PaymentTransactions.FindAsync(transactionId);
        }

        public async Task UpdateTransactionAsync(PaymentTransaction transaction)
        {
            _context.PaymentTransactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
