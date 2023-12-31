using Microsoft.EntityFrameworkCore;
using webAPI.Data;
using webAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ROWalksDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ROWalksConnectionString"));
});

builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();

// services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// app cors
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("corsapp");

app.UseAuthorization();

app.MapControllers();

app.Run();
