using Microsoft.EntityFrameworkCore;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Enrolment> Enrolments { get; set; }
}

// Minimal model definitions to satisfy compilation. Move these to separate files as appropriate.
public class AppUser
{
    public int UserID { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Event
{
    public int EventID { get; set; }
    public int OrganiserID { get; set; }
    // Add other properties as needed
}

public class Enrolment
{
    public int EnrolmentID { get; set; }
    public int ParticipantID { get; set; }
    // Add other properties as needed
}