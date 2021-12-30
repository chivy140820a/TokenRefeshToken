using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenApp.Data;
using TokenApp.Entity;
using TokenApp.SerVice.Token;
using TokenApp.SerVice.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IUserSerVice, UserSerVice>();
builder.Services.AddTransient<ITokenSerVice, TokenSerVice>();
var secretKeyBytes = Encoding.UTF8.GetBytes("Contentgydabdaiodhandjdha");
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(opt =>
              {
                  opt.TokenValidationParameters = new TokenValidationParameters
                  {
                      //tự cấp token
                      ValidateIssuer = false,
                      ValidateAudience = false,

                      //ký vào token
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                      ClockSkew = TimeSpan.Zero
                  };
              });
builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(
                   builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
