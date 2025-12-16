using Domin.Models;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddTransactionAsync(PaymentTransaction transaction);
        Task<PaymentTransaction> GetTransactionByIdAsync(int transactionId);
        Task UpdateTransactionAsync(PaymentTransaction transaction);
    }
}
