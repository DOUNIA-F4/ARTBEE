using ARTBEE_API.Helper;
using ARTBEE_API.Interfaces;
using ARTBEE_API.Models;
using ARTBEE_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IAuthService, AuthService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(a => 
    a.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Description = "JWT Authorization0",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
}));
builder.Services.AddDbContext<ArtBeeDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("con"))); // use your connection string
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

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwtOp =>
    {
        {
            //c.RequireHttpsMetadata = false;
            jwtOp.SaveToken = true;
            jwtOp.RequireHttpsMetadata = false;
            jwtOp.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            };
        }
    });
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
