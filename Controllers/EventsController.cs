using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    public EventsController(ApplicationDBContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetEvents() => Ok(await _context.Events.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> CreateEvent(Event newEvent)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser") return Unauthorized("Organisers only");

        newEvent.OrganiserID = HttpContext.Session.GetInt32("UserID") ?? 0;
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return Created($"/api/events/{newEvent.EventID}", newEvent);
    }
}