using ItemListDTO.Services;
using Spendy.Shared;

IConfiguration config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var connectionString = config.GetConnectionString("SpendyConnection");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSpendyServices(connectionString);
builder.Services.AddScoped<IItemListService, ItemListService>();
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