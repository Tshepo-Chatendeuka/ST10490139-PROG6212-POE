using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EnrolmentsController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    public EnrolmentsController(ApplicationDBContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> Enrol(Enrolment enrolment)
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant") return Unauthorized("Participants only");

        enrolment.ParticipantID = HttpContext.Session.GetInt32("UserID") ?? 0;
        _context.Enrolments.Add(enrolment);
        await _context.SaveChangesAsync();
        return Created("", enrolment);
    }

    [HttpGet("my-events")]
    public async Task<IActionResult> GetMyEvents()
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant") return Unauthorized("Participants only");
        int userId = HttpContext.Session.GetInt32("UserID") ?? 0;
        var enrolments = await _context.Enrolments.Where(e => e.ParticipantID == userId).ToListAsync();
        return Ok(enrolments);
    }
}