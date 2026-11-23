
using System.Collections.Concurrent;
using UserService.Models;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly ConcurrentDictionary<int, User> Users = new();
    private static int _nextId = 2;

    static UsersController()
    {
        Users.TryAdd(1, new User
        {
            Id = 1,
            FirstName = "Pradeep",
            LastName = "Singh",
            Email = "pradeep@example.com"
        });

        Users.TryAdd(2, new User
        {
            Id = 2,
            FirstName = "Rahul",
            LastName = "Sharma",
            Email = "rahul@example.com"
        });
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAll()
    {
        return Ok(Users.Values.OrderBy(u => u.Id));
    }

    [HttpGet("{id:int}")]
    public ActionResult<User> GetById(int id)
    {
        if (!Users.TryGetValue(id, out var user))
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public ActionResult<User> Create(CreateUserRequest request)
    {
        var user = new User
        {
            Id = Interlocked.Increment(ref _nextId),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };

        Users.TryAdd(user.Id, user);

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            user);
    }

    [HttpPut("{id:int}")]
    public ActionResult<User> Update(
        int id, CreateUserRequest request)
    {
        if (!Users.TryGetValue(id, out var existing))
            return NotFound();

        var updatedUser = new User
        {
            Id = id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            CreatedAt = existing.CreatedAt
        };

        Users[id] = updatedUser;

        return Ok(updatedUser);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!Users.TryRemove(id, out _))
            return NotFound();

        return NoContent();
    }
}
