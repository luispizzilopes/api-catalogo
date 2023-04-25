using ApiCatalogo.Context;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Filters;
using ApiCatalogo.Repository.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiCatalogo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //Adicionando o serviço do Unit of Work 
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Adicionando o serviço do Filtro personalizado
            builder.Services.AddScoped<ApiLogginFilter>();

            //Adicionando o serviço do AutoMapper
            var mappingConfig = new MapperConfiguration(mp =>
            {
                mp.AddProfile(new MappingProfile()); 
            });

            IMapper mapper = mappingConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);


            builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); 

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiCatalogo", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Header de autorização JWT usando o esquema Bearer.\r\n\r\nInforme 'Bearer'[espaço] e o seu token.\r\n\r\nExamplo: \'Bearer 12345abcdef\'",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            //Inclusão do serviço do contexto do EF
            //Defininfo a string de conexão com o banco de dados
            string mySqlConnetion = builder.Configuration.GetConnectionString("DefaultConnetion");
            //Registro do contexto da EF
            builder.Services.AddDbContext<AppDbContext>(options =>
              options.UseMySql(mySqlConnetion, ServerVersion.AutoDetect(mySqlConnetion)));

            //Adicionando o serviço de Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            /*
             JWT - Adiciona o manipulador de autenticação e define o 
            esquema de autenticação usando : Beader
            valida o emissor, a audiencia e a chave 
            usando a chave secreta valida a assinatura
             */
            builder.Services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
                    ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:key"]))
                }); 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}