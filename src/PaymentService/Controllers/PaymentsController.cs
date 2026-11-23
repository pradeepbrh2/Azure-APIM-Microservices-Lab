
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private static readonly ConcurrentDictionary<int, Payment> Payments = new();
    private static int _nextId = 2;

    static PaymentsController()
    {
        Payments.TryAdd(1, new Payment
        {
            Id = 1,
            OrderId = 1,
            Amount = 65000,
            PaymentMethod = "UPI",
            Status = "Completed"
        });

        Payments.TryAdd(2, new Payment
        {
            Id = 2,
            OrderId = 2,
            Amount = 3000,
            PaymentMethod = "Card",
            Status = "Pending"
        });
    }

    [HttpGet]
    public ActionResult<IEnumerable<Payment>> GetAll()
    {
        return Ok(Payments.Values.OrderBy(p => p.Id));
    }

    [HttpGet("{id:int}")]
    public ActionResult<Payment> GetById(int id)
    {
        if (!Payments.TryGetValue(id, out var payment))
            return NotFound();

        return Ok(payment);
    }

    [HttpPost]
    public ActionResult<Payment> Create(CreatePaymentRequest request)
    {
        var payment = new Payment
        {
            Id = Interlocked.Increment(ref _nextId),
            OrderId = request.OrderId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            Status = "Pending"
        };

        Payments.TryAdd(payment.Id, payment);

        return CreatedAtAction(
            nameof(GetById),
            new { id = payment.Id },
            payment);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Payment> Update(
        int id, CreatePaymentRequest request)
    {
        if (!Payments.TryGetValue(id, out var existing))
            return NotFound();

        var updatedPayment = new Payment
        {
            Id = id,
            OrderId = request.OrderId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            Status = existing.Status,
            CreatedAt = existing.CreatedAt
        };

        Payments[id] = updatedPayment;

        return Ok(updatedPayment);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!Payments.TryRemove(id, out _))
            return NotFound();

        return NoContent();
    }
}
