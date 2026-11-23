
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly ConcurrentDictionary<int, Order> Orders = new();
    private static int _nextId = 2;

    static OrdersController()
    {
        Orders.TryAdd(1, new Order
        {
            Id = 1,
            UserId = 1,
            ProductName = "Laptop",
            Quantity = 1,
            UnitPrice = 65000,
            TotalAmount = 65000
        });

        Orders.TryAdd(2, new Order
        {
            Id = 2,
            UserId = 2,
            ProductName = "Keyboard",
            Quantity = 2,
            UnitPrice = 1500,
            TotalAmount = 3000
        });
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetAll()
    {
        return Ok(Orders.Values.OrderBy(o => o.Id));
    }

    [HttpGet("{id:int}")]
    public ActionResult<Order> GetById(int id)
    {
        if (!Orders.TryGetValue(id, out var order))
            return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public ActionResult<Order> Create(CreateOrderRequest request)
    {
        var order = new Order
        {
            Id = Interlocked.Increment(ref _nextId),
            UserId = request.UserId,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            TotalAmount = request.Quantity * request.UnitPrice
        };

        Orders.TryAdd(order.Id, order);

        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id },
            order);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Order> Update(
        int id, CreateOrderRequest request)
    {
        if (!Orders.TryGetValue(id, out var existing))
            return NotFound();

        var updatedOrder = new Order
        {
            Id = id,
            UserId = request.UserId,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            TotalAmount = request.Quantity * request.UnitPrice,
            Status = existing.Status,
            CreatedAt = existing.CreatedAt
        };

        Orders[id] = updatedOrder;

        return Ok(updatedOrder);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!Orders.TryRemove(id, out _))
            return NotFound();

        return NoContent();
    }
}
