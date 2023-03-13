using ApiCatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Inclusão do serviço do contexto do EF
            //Defininfo a string de conexão com o banco de dados
            string mySqlConnetion = builder.Configuration.GetConnectionString("DefaultConnetion");
            //Registro do contexto da EF
            builder.Services.AddDbContext<AppDbContext>(options =>
              options.UseMySql(mySqlConnetion, ServerVersion.AutoDetect(mySqlConnetion))); 

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
        }
    }
}