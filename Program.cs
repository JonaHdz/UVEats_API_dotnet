using UVEATS_API_DOTNET.Models;
using UVEATS_API_DOTNET.Business;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//SERVICIOS DENUESTRA API
builder.Services.AddDbContext<UveatsContext>(options =>
                options.UseMySql(
                    "server=localhost;database=Cursos;uid=root;pwd=74ls00", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.25-mysql"
                )));
//AGREGAR EN ESTA LINEA LOS DEMAS PROVIDER NECESARIOS                
builder.Services.AddScoped<LoginProvider>();
builder.Services.AddScoped<CategoriaProvider>();
builder.Services.AddScoped<UsuarioProvider>();
builder.Services.AddScoped<EmpleadoProvider>();

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
