using Domin.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.Implementation;
using Repository;
using Domin.Enum;
using Domin.Helper;
using Microsoft.EntityFrameworkCore;
using MailKit.Search;
using System.Numerics;
using Domin.paymentclasses;
using Domin.webhook;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly ApplicationDbContext _db;
    private readonly PaymobSettings _settings;


    public PaymentsController(PaymentService paymentService, ApplicationDbContext db, PaymobSettings settings)
    {
        _paymentService = paymentService;
        _db = db;
        _settings = settings;
    }

    [HttpPost("pay-now")]
    public async Task<IActionResult> PayNow([FromBody]PayNowRequest request)
    {
        var res = await _paymentService.CreatePaymentAsync(request);
        return Ok(new { res });
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook([FromBody] PaymobWebhookModel payload)
    {
        try
        {
            if (payload?.obj == null)
                return BadRequest("Invalid payload");

            var paymobOrderId = payload.obj.order.id;
            var paymobTransactionId = payload.obj.Transactionid;
            var success = payload.obj.success;
            var amount = payload.obj.amount_cents / 100m;

          
            var transaction = await _db.PaymentTransactions
                .FirstOrDefaultAsync(t => t.PaymobOrderId == paymobOrderId);

            if (transaction == null)
            {
                
                throw new Exception("transaction not found");
            }
            else
            {
                
                transaction.PaymobTransactionId = paymobTransactionId;
                transaction.Status = PaymentStatus.Pending;
                transaction.PaidAt = success ? DateTime.UtcNow : null;
                transaction.Amount = amount;
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Webhook received successfully",
                success,
                paymobOrderId,
                paymobTransactionId
            });
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, new { error = ex.Message });
        }
    }
}



    
