using UVEATS_API_DOTNET.Models;
using UVEATS_API_DOTNET.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;



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
builder.Services.AddScoped<PedidoProvider>();
builder.Services.AddScoped<ProductoProvider>();
builder.Services.AddScoped<ResenaProvider>();
builder.Services.AddScoped<ConversacionPedidoProvider>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false,//aqui seria el rol del ususario
        };
    });


var app = builder.Build();

//
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();//para el token

app.UseAuthorization();

app.MapControllers();

app.Run();
