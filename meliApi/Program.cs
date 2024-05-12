using meliApi.Data;
using meliApi.Services.Implementacion;
using meliApi.Services.Interfaces;
using meliApi.Servicios;
using Microsoft.EntityFrameworkCore;

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


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("name=DefaultConnection");
            });
            builder.Services.AddScoped<MeliTokenServicio>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://example.com")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");
            app.MapControllers();

            app.Run();
        }
    }
}