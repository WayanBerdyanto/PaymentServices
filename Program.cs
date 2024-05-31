using PaymentServices.DAL;
using PaymentServices.DAL.Interfaces;
using PaymentServices.DTO;
using PaymentServices.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUsers, UserDAL>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users", (IUsers user) =>
{
    return Results.Ok(user.GetAll());
});

app.MapGet("/users/{name}", (IUsers user, string name) =>
{
    var users = user.GetByName(name);
    if (users == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(users);
});

app.MapPost("/user", (IUsers user, Users obj) =>
{
    try
    {
        Users users = new Users
        {
            UserName = obj.UserName,
            Password = obj.Password,
            FullName = obj.FullName,
            Balance = obj.Balance
        };
        user.Insert(users);
        return Results.Created($"/user/{users.UserName}", users);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/users/updatebalance", async (IUsers userDal, UserUpdateBalanceDTO userDto) =>
{
    try
    {
        await userDal.UpdateBalancekAsync(userDto.UserName, userDto.Balance);
        return Results.Ok(new { Message = "Users Balance updated successfully" });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { Message = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { Message = "An error occurred while updating the Users Balance", Error = ex.Message });
    }
});


app.MapPost("/login", async (IUsers user, Users login) =>
{
    try
    {
        var users = user.ValidateUser(login.UserName, login.Password);
        if (users != null)
        {
            var token = GenerateJwtToken(users, builder.Configuration);
            return Results.Ok(new  { Message = "Login Berhasil", Token = token });
        }
        else
        {
            return Results.Unauthorized();
        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.Run();

string GenerateJwtToken(Users user, IConfiguration configuration)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
        issuer: configuration["Jwt:Issuer"],
        audience: configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(int.Parse(configuration["Jwt:ExpiryMinutes"])),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
