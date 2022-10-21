using ARTBEE_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ArtBeeDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString(""))); // use your connection string
builder.Services.AddIdentity<User, IdentityRole>(conf =>
{
    conf.Password.RequiredLength = 4;
    conf.Password.RequireUppercase = false;
    conf.Password.RequireNonAlphanumeric = false;
    conf.Password.RequiredUniqueChars = 0;
    conf.Password.RequireDigit = false;
    conf.SignIn.RequireConfirmedAccount = false;
    conf.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ArtBeeDbContext>();

builder.Services.AddControllers();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
