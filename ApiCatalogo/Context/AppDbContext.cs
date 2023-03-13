using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Context
{
    public class AppDbContext : DbContext
    {
        //Definindo no construtor a configuração do contexto utilizando o EntityFrameWork
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //Mapeamento das entidades
        public DbSet<Categoria>? Categorias { get; set; }   
        public DbSet<Produto>? Produtos { get; set; }
    }
}
