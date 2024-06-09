using PaymentServices.DAL;
using PaymentServices.DAL.Interfaces;
using PaymentServices.DTO;
using PaymentServices.Models;
using PaymentServices.Services;
using Polly;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IDetailPayment, DetailPaymentDAL>();

//register HttpClient
builder.Services.AddHttpClient<IUserServices, UserService>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(6000)));

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


app.MapGet("/detailPayment", (IDetailPayment detailPayment) =>
{
    return Results.Ok(detailPayment.GetAll());
});

app.MapPost("/detailPayment", async (IDetailPayment iDetailPayment, InsertDetailPaymentDTO obj, IUserServices userServices) =>
{
    var user = await userServices.GetUserByName(obj.UserName);
    if (user == null)
    {
        return Results.BadRequest("data not found");
    }
    try
    {
        DetailPayment detail = new DetailPayment
        {
            PaymentMethod = obj.PaymentMethod,
            UserName = obj.UserName,
            Amount = obj.Amount,
            DateTopUp = obj.DateTopUp
        };
        UserUpdateBalanceDTO userUpdateBalance = new UserUpdateBalanceDTO
        {
            userName = obj.UserName,
            Balance = obj.Amount 
        };

        iDetailPayment.Insert(detail);
        await userServices.TopUpBalanceAsync(userUpdateBalance);
        return Results.Created($"/detailPayment/{detail.DetailPaymentId}", detail);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

