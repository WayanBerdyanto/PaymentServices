using PaymentServices.DAL;
using PaymentServices.DAL.Interfaces;
using PaymentServices.DTO;
using PaymentServices.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUsers, UserDAL>();


// Add services to the container.
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
            return Results.Ok(users);
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

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
