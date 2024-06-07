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

builder.Services.AddScoped<IPaymentMethod, PaymentMethodDAL>();

builder.Services.AddScoped<IDetailPayment, DetailPaymentDAL>();

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


app.MapGet("/paymentMethod", (IPaymentMethod paymentMethod) =>
{
    return Results.Ok(paymentMethod.GetAll());
});

app.MapPost("/paymentMethod", (IPaymentMethod paymentMethod, PaymentMethod obj) =>
{
    try
    {
        PaymentMethod payment = new PaymentMethod
        {
            NamePayment = obj.NamePayment
        };
        paymentMethod.Insert(payment);
        return Results.Created($"/user/{payment.IdPayment}", payment);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/paymentMethod", (IPaymentMethod paymentMethod, PaymentMethod obj) =>
{
    try
    {
        PaymentMethod payment = new PaymentMethod
        {
            NamePayment = obj.NamePayment
        };
        paymentMethod.Update(payment);
        return Results.Ok(payment);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/paymentMethod/{id}", (IPaymentMethod paymentMethod, int id) =>
{
    try
    {
        paymentMethod.Delete(id);
        return Results.Ok(new { success = true, message = "request delete successful" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/detailPaymentMethod", (IDetailPayment detailPayment) =>
{
    return Results.Ok(detailPayment.GetAll());
});

app.Run();

