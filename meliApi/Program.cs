using meliApi.Controllers;
using meliApi.Data;
using meliApi.Data.Repositories.Implementacion;
using meliApi.Data.Repositories.Interface;
using meliApi.Servicios;
using meliApi.Servicios.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace meliApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<UsuarioServicio>();
            builder.Services.AddScoped<TokenServicios>();
            builder.Services.AddScoped<MeliController>();
            builder.Services.AddScoped<ItemService>();
            builder.Services.AddScoped<IItemCollection, ItemCollection>();
            builder.Services.AddHttpClient<IItemService, ItemService>();
            builder.Services.AddScoped<IProductosCollection, ProductoCollection>();

            // Configuración de autenticación JWT
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("key"))
                    };
                });

            // Configuración del DbContext de MySQL
            builder.Services.AddDbContext<MySqlDbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                                 new MySqlServerVersion(new Version(8, 0, 21)));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.MapControllers();

            app.Run();
        }
    }
}