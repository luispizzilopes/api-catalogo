using ApiCatalogo.Context;
using ApiCatalogo.Controllers;
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Repository.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoXUnitTestes
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper; 
        private IUnitOfWork unitOfWork;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost;Database=ApiCatalogoDB;Uid=luis;Pwd=1203";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options; 
        }

        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            unitOfWork = new UnitOfWork(context);
        }

        //Teste unitários
        //Testar o método GET
        [Fact]
        public void GetCategorias_Return_OkResult()
        {
            //Arrange

            //Act
     
            //Assert
         
        }
    }
}
