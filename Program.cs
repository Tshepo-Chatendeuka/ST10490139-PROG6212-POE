using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Database Context
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Session Configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Swagger registration removed - install Swashbuckle.AspNetCore to enable AddSwaggerGen

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Swagger middleware disabled - install Swashbuckle.AspNetCore to enable UseSwagger / UseSwaggerUI
}

app.UseHttpsRedirection();
app.UseSession(); // Enable Session
app.UseAuthorization();
app.MapControllers();
app.Run();